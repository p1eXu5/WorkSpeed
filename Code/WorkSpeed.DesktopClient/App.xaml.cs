using System.Windows;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.DesktopClient.ViewModels;
using WorkSpeed.DesktopClient.Views;
using WorkSpeed.Productivity;

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

            var context = new WarehouseService();
            var calculator = new ProductivityCalculator();

            var mvm = new MainViewModel();
            mainWindow.DataContext = mvm;

            mainWindow.Show();
        }
    }
}
