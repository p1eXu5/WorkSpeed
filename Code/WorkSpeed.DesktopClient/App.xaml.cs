using System;
using System.Windows;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.DesktopClient.ViewModels;
using WorkSpeed.DesktopClient.Views;
using WorkSpeed.FileModels;

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

            ImportService importService = null; 
            try {
                importService = WorkSpeedBusinessContextCreator.Create();
                var mvm = new MainViewModel( importService );

                mainWindow.DataContext = mvm;
                mainWindow.Show();
            }
            finally {
                importService?.Dispose();
            }
        }

        
    }
}
