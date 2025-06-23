using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MY_PROJ32VAR.ViewModels.Commands;

namespace MY_PROJ32VAR.ViewModels
{
    public class SelectAllViewModel : ViewModelBase
    {
        private int _targetNumerator;
        private int _targetDenominator;
        private string _resultMessage = string.Empty;
        private int _score;
        private int _attempts;
        private const int MaxAttempts = 3;
        private ObservableCollection<FigureViewModel> _figures = new();
        public int TargetNumerator { get => _targetNumerator; set { _targetNumerator = value; OnPropertyChanged(); OnPropertyChanged(nameof(TargetFractionText)); } }
        public int TargetDenominator { get => _targetDenominator; set { _targetDenominator = value; OnPropertyChanged(); OnPropertyChanged(nameof(TargetFractionText)); } }
        public string TargetFractionText => $"{TargetNumerator}/{TargetDenominator}";
        public ObservableCollection<FigureViewModel> Figures { get => _figures; set { _figures = value; OnPropertyChanged(); } }
        public int Score { get => _score; set { _score = value; OnPropertyChanged(); } }
        public string ResultMessage { get => _resultMessage; set { _resultMessage = value; OnPropertyChanged(); } }
        public ICommand ToggleSelectCommand { get; }
        public ICommand CheckCommand { get; }
        public SelectAllViewModel()
        {
            ToggleSelectCommand = new RelayCommand(f => f is FigureViewModel, f => ToggleSelect((FigureViewModel)f));
            CheckCommand = new RelayCommand(_ => Figures.Any(x => x.IsSelected), _ => Check());
            StartNewRound();
        }
        private void StartNewRound()
        {
            _attempts = 0;
            ResultMessage = string.Empty;
            foreach (var f in Figures) { f.IsSelected = false; f.IsCorrect = false; f.IsWrong = false; }
            GenerateTask();
        }
        private void GenerateTask()
        {
            var rnd = new Random();
            TargetDenominator = rnd.Next(2, 13);
            TargetNumerator = rnd.Next(1, TargetDenominator / 2 + 1);
            double targetProp = (double)TargetNumerator / TargetDenominator;
            var figs = new ObservableCollection<FigureViewModel>();
            // Добавляем 2-3 правильных фигуры
            int correctCount = rnd.Next(2, 4);
            int guaranteed = 0;
            for (int i = 0; i < correctCount; i++)
            {
                int den;
                do { den = rnd.Next(2, 13); } while (den == TargetDenominator || figs.Any(f => f.SectorCount == den && f.Filled == (int)Math.Round(targetProp * den)));
                int num = (int)Math.Round(targetProp * den);
                if (num == 0 || num == den) { i--; continue; }
                figs.Add(new FigureViewModel(den, num));
                guaranteed++;
            }
            // Добавляем неправильные фигуры
            while (figs.Count < 6)
            {
                int den = rnd.Next(2, 13);
                int num = rnd.Next(1, den);
                double prop = (double)num / den;
                if (Math.Abs(prop - targetProp) < 0.01 || figs.Any(f => f.SectorCount == den && f.Filled == num)) continue;
                figs.Add(new FigureViewModel(den, num));
            }
            // Гарантия: если вдруг нет ни одной правильной фигуры, добавляем одну
            if (!figs.Any(f => Math.Abs((double)f.Filled / f.SectorCount - targetProp) < 0.01))
            {
                int den = TargetDenominator;
                int num = TargetNumerator;
                figs[0] = new FigureViewModel(den, num);
            }
            Figures = new ObservableCollection<FigureViewModel>(figs.OrderBy(_ => rnd.Next()));
        }
        private void ToggleSelect(FigureViewModel fig)
        {
            fig.IsSelected = !fig.IsSelected;
            OnPropertyChanged(nameof(Figures));
        }
        private void Check()
        {
            double targetProp = (double)TargetNumerator / TargetDenominator;
            bool allCorrect = true;
            foreach (var fig in Figures)
            {
                double prop = (double)fig.Filled / fig.SectorCount;
                bool isCorrect = Math.Abs(prop - targetProp) < 0.01;
                if (fig.IsSelected && isCorrect)
                {
                    fig.IsCorrect = true;
                    fig.IsWrong = false;
                }
                else if (fig.IsSelected && !isCorrect)
                {
                    fig.IsCorrect = false;
                    fig.IsWrong = true;
                    allCorrect = false;
                }
                else if (!fig.IsSelected && isCorrect)
                {
                    fig.IsCorrect = false;
                    fig.IsWrong = true;
                    allCorrect = false;
                }
                else
                {
                    fig.IsCorrect = false;
                    fig.IsWrong = false;
                }
            }
            _attempts++;
            if (allCorrect)
            {
                ResultMessage = "Верно!";
                Score++;
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
                {
                    App.Current.Dispatcher.Invoke(StartNewRound);
                });
            }
            else
            {
                // После первого неверного ответа подсвечиваем правильные варианты
                foreach (var fig in Figures)
                {
                    double prop = (double)fig.Filled / fig.SectorCount;
                    bool isCorrect = Math.Abs(prop - targetProp) < 0.01;
                    if (isCorrect) fig.IsCorrect = true;
                    else fig.IsWrong = true;
                }
                ResultMessage = "Неверно. Правильные варианты подсвечены.";
                OnPropertyChanged(nameof(Figures));
                System.Threading.Tasks.Task.Delay(1500).ContinueWith(_ =>
                {
                    App.Current.Dispatcher.Invoke(StartNewRound);
                });
            }
            OnPropertyChanged(nameof(Figures));
        }
    }
} 