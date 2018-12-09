using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class DataImportViewModel : ViewModel
    {
        private readonly IWarehouse _warehouse;
        private readonly ObservableCollection<EmployeeViewModel> _unknownVmCollection;

        public DataImportViewModel (IWarehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));

            _unknownVmCollection = new ObservableCollection<EmployeeViewModel>();
            UnknownEmployeeVmCollection = new ReadOnlyObservableCollection<EmployeeViewModel>(_unknownVmCollection);
            Observe (_warehouse.NewData.Employees, _unknownVmCollection, e => e.Employee);

        }

        public ReadOnlyObservableCollection<EmployeeViewModel> UnknownEmployeeVmCollection { get; }
    
        
        public ICommand OpenFileCommand => new MvvmCommand (OpenFile);

        private void OpenFile (object obj)
        {
            var ofd = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyComputer),
                    Filter = "Excel Files|*.xls;*.xlsx",
                    RestoreDirectory = true,
                };

            if (true == ofd.ShowDialog()) {

                _warehouse.ImportAsync (ofd.FileName);
            }
        }
    }
}
