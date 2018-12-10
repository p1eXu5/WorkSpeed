using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IWarehouse _warehouse;

        public MainViewModel(IWarehouse warehouse)
        {
            _warehouse = warehouse;
            ImportVm = new ImportViewModel (warehouse);
        }

        public ImportViewModel ImportVm { get; set; }
    }
}
