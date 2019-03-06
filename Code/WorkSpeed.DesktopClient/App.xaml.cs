using System;
using System.IO;
using System.Windows;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts;
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
            base.OnStartup( e );

            try {
                var (importService, reportService) = WorkSpeedBusinessContextCreator.Create();
                reportService.ReloadAllCollections();              

                Window mainWindow = new MainWindow();
                var dialogRepository = new DialogRepository( mainWindow );
                dialogRepository.Register< ErrorViewModel, ErrorWindow >();
                var mvm = new MainViewModel( importService, reportService, dialogRepository );
                mainWindow.DataContext = mvm;

                mainWindow.Show();
            }
            catch
#if RELEASE
                ( Exception ex ) 
#endif
            {
#if DEBUG
                throw;
#endif

#if RELEASE
                string message = $"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}";
                File.AppendAllText( "log.log", message );
#endif
            }
        }
    }
}
