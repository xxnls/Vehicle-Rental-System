using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackOffice.Views.Other
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void ChangeLanguage_OnClick(object sender, RoutedEventArgs e)
        {
            // Refreshes the window after changing language
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).Close();
                var newWindow = new MainWindow();
                newWindow.Show();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
