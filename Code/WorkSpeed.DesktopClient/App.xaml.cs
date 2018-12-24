using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            var sw = new Stopwatch();
            sw.Start();

            object typeInstance = Activator.CreateInstance( typeof( ProductImportModel) );

            Debug.WriteLine( sw.Elapsed );

            var vm = new FastProductivityViewModel();
            Window mainWindow = new FastProductivityMainWindow();
            mainWindow.DataContext = vm;

            mainWindow.Show();
        }
    }
}
