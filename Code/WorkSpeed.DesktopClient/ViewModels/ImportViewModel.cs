using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class ImportViewModel : ViewModel
    {
        private readonly IWarehouse _warehouse;
        private readonly ObservableCollection<EmployeeViewModel> _unknownVmCollection;

        private bool _isFileProcessing;

        public ImportViewModel (IWarehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));

            _unknownVmCollection = new ObservableCollection<EmployeeViewModel>();
            UnknownEmployeeVmCollection = new ReadOnlyObservableCollection<EmployeeViewModel>(_unknownVmCollection);
            Observe (_warehouse.NewData.Employees, _unknownVmCollection, e => e.Employee);

        }

        public ReadOnlyObservableCollection<EmployeeViewModel> UnknownEmployeeVmCollection { get; }

        public bool IsFileProcessing
        {
            get => _isFileProcessing;
            set {
                _isFileProcessing = value;
                OnPropertyChanged ();
            }
        }
        
        public ICommand OpenFileCommand => new MvvmCommand (OpenFile);

        private async void OpenFile (object obj)
        {
            var ofd = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyComputer),
                    Filter = "Excel Files|*.xls;*.xlsx",
                    RestoreDirectory = true,
                };

            if (true == ofd.ShowDialog()) {

                _isFileProcessing = true;
                ((MvvmCommand)OpenFileCommand).RaiseCanExecuteChanged();

                await _warehouse.ImportAsync (ofd.FileName);

                _isFileProcessing = false;
                ((MvvmCommand)OpenFileCommand).RaiseCanExecuteChanged();
            }
        }

        private bool IsFileProcess() => !_isFileProcessing;
    }
}
