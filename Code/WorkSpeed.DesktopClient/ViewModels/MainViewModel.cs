using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IWarehouse _warehouse;
        private readonly IDialogRepository _viewRepository;

        public MainViewModel( IWarehouse warehouse, IDialogRepository viewRepository )
        {
            _warehouse = warehouse;
            ImportVm = new ImportViewModel (warehouse);
        }

        public ImportViewModel ImportVm { get; set; }

        public ICommand ExitCommand => new MvvmCommand( Exit );

        private void Exit( object obj )
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
