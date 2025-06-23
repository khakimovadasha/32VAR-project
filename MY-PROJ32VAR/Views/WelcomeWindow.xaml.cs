using System.Windows;

namespace MY_PROJ32VAR.Views
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Mode_Train_Click(object sender, RoutedEventArgs e)
        {
            var win = new BuildFractionWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void Mode_Pairs_Click(object sender, RoutedEventArgs e)
        {
            var win = new FindPairWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void Mode_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            var win = new SelectAllWindow();
            win.Owner = this;
            win.ShowDialog();
        }
    }
} 