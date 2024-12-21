using BackOffice.ViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackOffice.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeLanguage_OnClick(object sender, RoutedEventArgs e)
        {
            // Refreshes the window after changing language
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var newWindow = new MainWindow();
                newWindow.Show();
                this.Close();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}