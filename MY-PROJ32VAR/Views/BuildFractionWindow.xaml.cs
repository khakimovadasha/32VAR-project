using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using MY_PROJ32VAR.ViewModels;
using System.Windows.Shapes;

namespace MY_PROJ32VAR.Views
{
    public partial class BuildFractionWindow : Window
    {
        public BuildFractionWindow()
        {
            InitializeComponent();
            DataContext = new BuildFractionViewModel();
        }

        private void Sector_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is BuildFractionViewModel vm && sender is Path path && path.DataContext is SectorViewModel svm)
            {
                svm.IsSelected = !svm.IsSelected;
            }
        }
    }
} 