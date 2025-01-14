using System.Configuration;
using System.Data;
using System.Windows;
using System.Globalization;
using BackOffice.Properties;
using BackOffice.Helpers;
using BackOffice.Views;
using BackOffice.Views.Other;
using CommunityToolkit.Mvvm.Messaging;

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

            LocalizationHelper.SetLanguage(Settings.Default.Language);

            // Show LoginWindow on startup
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            // Register for login success message
            WeakReferenceMessenger.Default.Register<Messenger>(this, (r, m) =>
            {
                if (m.Value == "LoginSuccessful")
                {
                    //loginWindow.Close();
                    Application.Current.Windows[0]?.Close();

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }

                if (m.Value == "LogoutSuccessful")
                {
                    SessionManager.Clear();

                    Application.Current.Windows[0]?.Close();

                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                }
            });
        }
    }

}
