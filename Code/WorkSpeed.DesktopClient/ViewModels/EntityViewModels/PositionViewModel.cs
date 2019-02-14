using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class PositionViewModel : ViewModel
    {
        private readonly ObservableCollection< EmployeeViewModel > _employees;

        public PositionViewModel ( PositionGrouping position )
        {
            Position = position.Position;
            _employees = new ObservableCollection< EmployeeViewModel >( position.Employees.Select( e => new EmployeeViewModel( e ) ) );
            Employees = new ReadOnlyObservableCollection< EmployeeViewModel >( _employees );
        }

        public Position Position { get; }
        public ReadOnlyObservableCollection< EmployeeViewModel > Employees { get; }
    }
}
