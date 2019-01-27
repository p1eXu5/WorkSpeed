using System.Windows;
using WorkSpeed.DesktopClient.Views;

namespace WorkSpeed.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window mainWindow = new MainWindow();

            mainWindow.Show();
        }
    }
}
