using System.Windows;
using System.Windows.Controls;
using MY_PROJ32VAR.ViewModels;

namespace MY_PROJ32VAR.Views
{
    public partial class SelectAllView : UserControl
    {
        public SelectAllView()
        {
            InitializeComponent();
            if (DataContext == null)
                DataContext = new SelectAllViewModel();
        }
    }
} 