
using System.Windows;

namespace SettingUpSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();
            bool? result = loginWindow.ShowDialog(); 

            if (result == true && loginWindow.IsAuthenticated)
            {
                var mainWindow = new MainWindow();
                this.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Shutdown(); 
            }
        }
    }

}
