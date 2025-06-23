using System.Windows;
using MY_PROJ32VAR.ViewModels;

namespace MY_PROJ32VAR.Views
{
    public partial class FindPairWindow : Window
    {
        public FindPairWindow()
        {
            InitializeComponent();
            DataContext = new FindPairViewModel();
        }
    }
}