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

            CreateCollection();

            var view = SetupView( EmployeeVmCollection, predicate );
            view.SortDescriptions.Add( new SortDescription( "IsNotActive", ListSortDirection.Ascending ) );
            view.SortDescriptions.Add( new SortDescription( "SecondName", ListSortDirection.Ascending ) );


            void CreateCollection ()
            {
                var employeeVmCollection = new ObservableCollection< EmployeeViewModel >( positionGrouping.Employees.Select( e =>
                                                                                                                             {
                                                                                                                                 var evm = new EmployeeViewModel( e );
                                                                                                                                 evm.PropertyChanged += OnIsModifyChanged;
                                                                                                                                 return evm;
                                                                                                                             } ) );
                EmployeeVmCollection = new ReadOnlyObservableCollection< EmployeeViewModel >( employeeVmCollection );
            }
        }

        public Position Position { get; }
        public ReadOnlyObservableCollection< EmployeeViewModel > EmployeeVmCollection { get; private set; }

         


        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = EmployeeVmCollection.Any( evm => evm.IsModify );
        }

        protected internal override void Refresh ()
        {
            base.Refresh();
        }
    }
}
