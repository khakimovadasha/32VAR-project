using System.Windows;
using MY_PROJ32VAR.ViewModels;

namespace MY_PROJ32VAR.Views
{
    public partial class SelectAllWindow : Window
    {
        public SelectAllWindow()
        {
            InitializeComponent();
            DataContext = new SelectAllViewModel();
        }
    }
} 