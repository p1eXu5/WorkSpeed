using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Experiments.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup ( StartupEventArgs e )
        {
            var vm = new MainViewModel();

            new MainWindow {
                DataContext = vm,
            }.ShowDialog();
        }
    }
}
