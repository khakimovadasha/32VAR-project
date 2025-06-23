using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using MY_PROJ32VAR.ViewModels;

namespace MY_PROJ32VAR.Views
{
    public partial class BuildFractionView : UserControl
    {
        public BuildFractionView()
        {
            InitializeComponent();
        }

        public BuildFractionViewModel? ViewModel
        {
            get => DataContext as BuildFractionViewModel;
            set => DataContext = value;
        }

        private void Sector_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Path path && path.DataContext is SectorViewModel svm)
            {
                svm.IsSelected = !svm.IsSelected;
            }
        }
    }
} 