using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MY_PROJ32VAR.ViewModels
{
    public class FigureViewModel : ViewModelBase
    {
        public int Filled { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }
        private bool? _isCorrect;
        public bool? IsCorrect
        {
            get => _isCorrect;
            set { _isCorrect = value; OnPropertyChanged(); }
        }
        private bool _isWrong;
        public bool IsWrong
        {
            get => _isWrong;
            set { _isWrong = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SectorViewModel> Sectors { get; set; }
        public int SectorCount { get; set; }
        public FigureViewModel(int sectors, int filled)
        {
            SectorCount = sectors;
            Filled = filled;
            Sectors = new ObservableCollection<SectorViewModel>();
            for (int i = 0; i < sectors; i++)
                Sectors.Add(new SectorViewModel { Index = i, TotalSectors = sectors, IsSelected = i < filled });
        }
    }
} 