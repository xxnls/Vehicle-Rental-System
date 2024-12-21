using System.Configuration;
using System.Data;
using System.Windows;
using System.Globalization;
using BackOffice.Properties;

namespace BackOffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string savedLanguage = Settings.Default.Language;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(savedLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(savedLanguage);
        }
    }

}
