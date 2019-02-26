using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class PositionGroupingViewModel : FilteredViewModel
    {
        public PositionGroupingViewModel ( PositionGrouping positionGrouping, Predicate< object > predicate )
        {
            Position = positionGrouping.Position ?? throw new ArgumentNullException(nameof(positionGrouping), @"PositionGrouping cannot be null.");

            var employeeVmCollection = new ObservableCollection< EmployeeViewModel >( positionGrouping.Employees.Select( e => new EmployeeViewModel( e ) ) );
            EmployeeVmCollection = new ReadOnlyObservableCollection< EmployeeViewModel >( employeeVmCollection );

            var view = SetupView( EmployeeVmCollection, predicate );
            view.SortDescriptions.Add( new SortDescription( "Name", ListSortDirection.Ascending ) );
        }

        public Position Position { get; }
        public ReadOnlyObservableCollection< EmployeeViewModel > EmployeeVmCollection { get; }
    }
}
