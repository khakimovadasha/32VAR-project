using System.Collections.ObjectModel;
using System.ComponentModel;
using MY_PROJ32VAR.Models;
using System.Collections.Generic;

namespace MY_PROJ32VAR.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FractionTask> Tasks { get; set; }
        private int _currentTaskIndex;
        public int CurrentTaskIndex
        {
            get => _currentTaskIndex;
            set
            {
                if (_currentTaskIndex != value)
                {
                    _currentTaskIndex = value;
                    OnPropertyChanged(nameof(CurrentTaskIndex));
                    OnPropertyChanged(nameof(CurrentTask));
                }
            }
        }
        public object CurrentTask { get; set; }

        public MainViewModel()
        {
            Tasks = new ObservableCollection<FractionTask>
            {
                new FractionTask
                {
                    Numerator = 2,
                    Denominator = 5,
                    Type = TaskType.BuildFraction
                }
            };
            CurrentTaskIndex = 0;

            // Пример задания FindPair
            // var task = new FractionTask
            // {
            //     Numerator = 2,
            //     Denominator = 3,
            //     Type = TaskType.FindPair,
            //     VisualVariants = new List<string> { "circle_2_3", "circle_1_2", "grid_3_4", "circle_3_3", "grid_2_3", "circle_1_3" },
            //     CorrectAnswers = new List<int> { 0 }
            // };
            CurrentTask = new FindPairViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 