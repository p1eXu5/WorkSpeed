using System;
using System.Windows;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Factories;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.DesktopClient.ViewModels;
using WorkSpeed.DesktopClient.ViewModels.Dialogs;
using WorkSpeed.DesktopClient.Views;
using WorkSpeed.DesktopClient.Views.Dialogs;

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

            var dialogRepository = new DialogRepository( mainWindow );
            dialogRepository.Register< ErrorViewModel, ErrorWindow >();


            var (importService, reportService) = WorkSpeedBusinessContextCreator.Create();
            var mvm = new MainViewModel( importService, reportService, dialogRepository );

            mainWindow.DataContext = mvm;
            mainWindow.Show();
        }
    }
}
