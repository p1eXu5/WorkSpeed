using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using CircleDiagramTest.Models;
using CircleDiagramTest.ViewModels;

namespace CircleDiagramTest
{
    public class MainViewModel : ViewModel
    {
        private readonly ObservableCollection< EmployeeProductivityViewModel > _employeeProductivityVms;
        private readonly ObservableCollection< OperationViewModel > _operationVms;

        public MainViewModel ()
        {
            _employeeProductivityVms = new ObservableCollection< EmployeeProductivityViewModel >();
            EmployeeProductivityVms = new ReadOnlyObservableCollection< EmployeeProductivityViewModel >( _employeeProductivityVms );

            _operationVms = new ObservableCollection< OperationViewModel >();
            OperationVms = new ReadOnlyObservableCollection< OperationViewModel >( _operationVms );

            ShowProductivity( null );
        }

        public ReadOnlyObservableCollection< EmployeeProductivityViewModel > EmployeeProductivityVms { get; }
        public ReadOnlyObservableCollection< OperationViewModel > OperationVms { get; }

        public ICommand ShowProductivityCommand => new MvvmCommand( ShowProductivity );

        #region Methods

        private void ShowProductivity ( object obj )
        {
            var operations = Operation.Operations;

            foreach ( var operation in operations ) {
                _operationVms.Add( new OperationViewModel( operation ) );
            }

            var categories = CategoryGroup.Categories;

            //if ( _employeeProductivityVms.Any() ) { _employeeProductivityVms.Clear(); }

            _employeeProductivityVms.Add( new EmployeeProductivityViewModel( EmployeeProductivity.EmployeeProductivities[0], operations, categories ) );
        }

        #endregion

    }
}
