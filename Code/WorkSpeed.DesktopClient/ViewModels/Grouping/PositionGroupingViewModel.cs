using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class PositionGroupingViewModel : ViewModel
    {
        private readonly ObservableCollection< EmployeeViewModel > _employees;

        public PositionGroupingViewModel ( PositionGrouping positionGrouping )
        {
            Position = positionGrouping.Position ?? throw new ArgumentNullException(nameof(positionGrouping), @"PositionGrouping cannot be null."); ;
            _employees = new ObservableCollection< EmployeeViewModel >( positionGrouping.Employees.Select( e => new EmployeeViewModel( e ) ) );
            Employees = new ReadOnlyObservableCollection< EmployeeViewModel >( _employees );
        }

        public Position Position { get; }
        public ReadOnlyObservableCollection< EmployeeViewModel > Employees { get; }
    }
}
