using System.Collections.ObjectModel;
using System.Windows.Input;
using MY_PROJ32VAR.Models;
using MY_PROJ32VAR.ViewModels.Commands;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MY_PROJ32VAR.ViewModels
{
    public class BuildFractionViewModel : ViewModelBase
    {
        private FractionTask? _task;
        private int _numerator;
        private int _denominator;
        private ObservableCollection<SectorViewModel> _sectors = new ObservableCollection<SectorViewModel>();
        private int _targetNumerator;
        private int _targetDenominator;
        private string _resultMessage = string.Empty;
        private bool _isSuccess;

        public BuildFractionViewModel(FractionTask? task = null)
        {
            if (task != null)
            {
                _task = task;
                Numerator = task.Numerator;
                Denominator = task.Denominator;
            }
            GenerateRandomFraction();
            IncreaseDenominatorCommand = new RelayCommand(_ => SelectedCount < TargetDenominator, _ => AddSector());
            DecreaseDenominatorCommand = new RelayCommand(_ => SelectedCount > 0, _ => RemoveSector());
            CheckCommand = new RelayCommand(_ => true, _ => Check());
            UpdateSectors();
        }

        public BuildFractionViewModel() : this(new Models.FractionTask { Numerator = 2, Denominator = 3 }) { }

        public int Numerator
        {
            get => _numerator;
            set
            {
                _numerator = value;
                OnPropertyChanged(nameof(Numerator));
            }
        }

        public int Denominator
        {
            get => _denominator;
            set
            {
                _denominator = value;
                OnPropertyChanged(nameof(Denominator));
            }
        }

        public ObservableCollection<SectorViewModel> Sectors
        {
            get => _sectors;
            set
            {
                _sectors = value;
                OnPropertyChanged(nameof(Sectors));
            }
        }

        public int TargetNumerator
        {
            get => _targetNumerator;
            set { _targetNumerator = value; OnPropertyChanged(nameof(TargetNumerator)); }
        }

        public int TargetDenominator
        {
            get => _targetDenominator;
            set { _targetDenominator = value; OnPropertyChanged(nameof(TargetDenominator)); }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set
            {
                _resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                OnPropertyChanged(nameof(IsSuccess));
            }
        }

        public ICommand IncreaseDenominatorCommand { get; }
        public ICommand DecreaseDenominatorCommand { get; }
        public ICommand CheckCommand { get; }

        public int SelectedCount => Sectors.Count(s => s.IsSelected);

        private void GenerateRandomFraction()
        {
            var rnd = new Random();
            TargetDenominator = rnd.Next(2, 13); // 2..12
            TargetNumerator = rnd.Next(1, TargetDenominator); // 1..(denominator-1)
            Numerator = 0;
            Denominator = TargetDenominator;
            UpdateSectors(); // Обновляем круг с новыми секторами
        }

        private void AddSector()
        {
            var firstUnselected = Sectors.FirstOrDefault(s => !s.IsSelected);
            if (firstUnselected != null)
                firstUnselected.IsSelected = true;
            Numerator = SelectedCount;
        }

        private void RemoveSector()
        {
            var lastSelected = Sectors.LastOrDefault(s => s.IsSelected);
            if (lastSelected != null)
                lastSelected.IsSelected = false;
            Numerator = SelectedCount;
        }

        private void UpdateSectors()
        {
            Sectors = new ObservableCollection<SectorViewModel>();
            for (int i = 0; i < Denominator; i++)
                Sectors.Add(new SectorViewModel { Index = i, TotalSectors = Denominator, IsSelected = false });
            Numerator = SelectedCount;
        }

        private void Check()
        {
            if (SelectedCount == TargetNumerator && Denominator == TargetDenominator)
            {
                ResultMessage = "Верно!";
                IsSuccess = true;
                // Генерируем новую дробь и круг через 1 секунду
                Task.Delay(1000).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GenerateRandomFraction();
                        ResultMessage = string.Empty;
                        IsSuccess = false;
                    });
                });
            }
            else
            {
                ResultMessage = "Неверно";
                IsSuccess = false;
                // Явное обновление UI
                OnPropertyChanged(nameof(ResultMessage));
                OnPropertyChanged(nameof(IsSuccess));
            }
        }
    }

    public class SectorViewModel : INotifyPropertyChanged
    {
        public int Index { get; set; }
        public int TotalSectors { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}