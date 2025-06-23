using System.Collections.ObjectModel;
using System.Windows.Input;
using MY_PROJ32VAR.Models;
using MY_PROJ32VAR.ViewModels.Commands;
using System;
using System.Linq;

namespace MY_PROJ32VAR.ViewModels
{
    public class FindPairViewModel : ViewModelBase
    {
        private ObservableCollection<FigureViewModel> _figures = new();
        private FigureViewModel? _firstSelected;
        private FigureViewModel? _secondSelected;
        private string _resultMessage = string.Empty;
        private bool? _isSuccess;
        private int _rewards;

        public ObservableCollection<FigureViewModel> Figures
        {
            get => _figures;
            set { _figures = value; OnPropertyChanged(); }
        }
        public FigureViewModel? FirstSelected
        {
            get => _firstSelected;
            set { _firstSelected = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCheck)); }
        }
        public FigureViewModel? SecondSelected
        {
            get => _secondSelected;
            set { _secondSelected = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCheck)); }
        }
        public string ResultMessage
        {
            get => _resultMessage;
            set { _resultMessage = value; OnPropertyChanged(); }
        }
        public bool? IsSuccess
        {
            get => _isSuccess;
            set { _isSuccess = value; OnPropertyChanged(); }
        }
        public int Rewards
        {
            get => _rewards;
            set { _rewards = value; OnPropertyChanged(); }
        }
        public bool CanCheck => FirstSelected != null && SecondSelected != null && FirstSelected != SecondSelected;

        public ICommand SelectFigureCommand { get; }
        public ICommand CheckCommand { get; }

        public FindPairViewModel()
        {
            SelectFigureCommand = new RelayCommand(f => f is FigureViewModel, f => SelectFigure((FigureViewModel)f));
            CheckCommand = new RelayCommand(_ => CanCheck, _ => Check());
            StartNewRound();
        }

        private void StartNewRound()
        {
            ResultMessage = string.Empty;
            IsSuccess = null;
            FirstSelected = null;
            SecondSelected = null;
            Figures.Clear();
            var rnd = new Random();
            // Гарантированно создаём пару с одинаковой долей, но разным количеством секторов
            int sectors1, sectors2, filled1, filled2;
            double proportion;
            do {
                sectors1 = rnd.Next(2, 7);
                filled1 = rnd.Next(1, sectors1); // не 0, не полная
                proportion = (double)filled1 / sectors1;
                sectors2 = rnd.Next(2, 7);
                filled2 = (int)Math.Round(proportion * sectors2);
            } while (sectors1 == sectors2 || filled2 == 0 || filled2 == sectors2 || Math.Abs(proportion - (double)filled2 / sectors2) > 0.01);
            var pair1 = new FigureViewModel(sectors1, filled1);
            var pair2 = new FigureViewModel(sectors2, filled2);
            Figures.Add(pair1);
            Figures.Add(pair2);
            // Добавляем еще 4 случайные фигуры, не совпадающие по доле с парой
            

            // ИСПРАВИЛА  добавляем 4 фигуры, не совпадающие по доле с уже добавленными
            int attempts = 0;
            while (Figures.Count < 6 && attempts < 200)
            {
                int s = rnd.Next(2, 7);
                int f = rnd.Next(1, s);
                double p = (double)f / s;

                // Проверка: не совпадает ли по доле с любой уже добавленной фигурой
                bool isDuplicate = Figures.Any(existing =>
                {
                    double existingP = (double)existing.Filled / existing.SectorCount;
                    return Math.Abs(p - existingP) < 0.01;
                });

                if (isDuplicate) { attempts++; continue; }

                Figures.Add(new FigureViewModel(s, f));
                attempts++;
            }


            Figures = new ObservableCollection<FigureViewModel>(Figures.OrderBy(_ => rnd.Next()));
            // Временный отладочный вывод долей в консоль
            foreach (var fig in Figures)
                System.Diagnostics.Debug.WriteLine($"Sectors: {fig.SectorCount}, Filled: {fig.Filled}, Proportion: {(double)fig.Filled / fig.SectorCount:F3}");
            OnPropertyChanged(nameof(Figures));
        }

        private void SelectFigure(FigureViewModel? fig)
        {
            if (fig == null) return;
            if (FirstSelected == null)
            {
                FirstSelected = fig;
                fig.IsSelected = true;
            }
            else if (SecondSelected == null && fig != FirstSelected)
            {
                SecondSelected = fig;
                fig.IsSelected = true;
            }
            else
            {
                // Сбросить выбор, если клик по третьей фигуре
                foreach (var f in Figures) f.IsSelected = false;
                FirstSelected = fig;
                SecondSelected = null;
                fig.IsSelected = true;
            }
        }

        private void Check()
        {
            if (!CanCheck) return;
            double p1 = (double)FirstSelected.Filled / FirstSelected.SectorCount;
            double p2 = (double)SecondSelected.Filled / SecondSelected.SectorCount;
            if (Math.Abs(p1 - p2) < 0.01 && FirstSelected.SectorCount != SecondSelected.SectorCount)
            {
                ResultMessage = "Ответ верный";
                IsSuccess = true;
                OnPropertyChanged(nameof(ResultMessage));
                OnPropertyChanged(nameof(IsSuccess));
                Rewards++;
                FirstSelected.IsCorrect = true;
                SecondSelected.IsCorrect = true;
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
                {
                    App.Current.Dispatcher.Invoke(StartNewRound);
                });
            }
            else
            {
                ResultMessage = "Неверно";
                IsSuccess = false;
                OnPropertyChanged(nameof(ResultMessage));
                OnPropertyChanged(nameof(IsSuccess));
                FirstSelected.IsCorrect = false;
                SecondSelected.IsCorrect = false;
            }
        }
    }
} 