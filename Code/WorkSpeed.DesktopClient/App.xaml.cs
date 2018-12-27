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
using NpoiExcel;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.DesktopClient.ViewModels;
using WorkSpeed.DesktopClient.Views;
using WorkSpeed.FileModels;
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
            var context = new RuntimeWorkSpeedBusinessContext();
            var categoryFilter = new CategoryFilter( context.GetCategories() );
            var pauseBetween = new PauseBetweenActions( new BreakRepository(), TimeSpan.FromHours( 4 ) );

            var factoryEmployeeAction = new FactoryEmployeeAction( pauseBetween, categoryFilter, TimeSpan.FromMinutes( 2 ) );

            var importer = new ExcelDataImporter();

            var warehouse = new Warehouse( context, importer, factoryEmployeeAction );

            var vm = new FastProductivityViewModel( warehouse );
            Window mainWindow = new FastProductivityMainWindow();
            mainWindow.DataContext = vm;

            mainWindow.Show();
        }
    }
}
