using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Grouping
{
    public class PositionGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        public PositionGroupingViewModel ( PositionGrouping positionGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Position = positionGrouping.Position ?? throw new ArgumentNullException(nameof(positionGrouping), @"PositionGrouping cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

            CreateCollection();

            SetupView( EmployeeVmCollection, ( vsc ) =>
                                            {
                                                vsc.SortDescriptions.Add( new SortDescription( "IsNotActive", ListSortDirection.Ascending ) );
                                                vsc.SortDescriptions.Add( new SortDescription( "SecondName", ListSortDirection.Ascending ) );
                                                vsc.Filter = PredicateFunc;
                                            } );


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

        protected override bool PredicateFunc ( object o )
        {
            if (!(o is EmployeeViewModel employee)) return false;

            var res = _filterVmCollection[ ( int )FilterIndexes.IsActive ].Entities.Any( obj => ( bool )(obj).Equals( employee.IsActive ) )
                      && _filterVmCollection[ ( int )FilterIndexes.IsSmoker ].Entities.Any( obj => ( bool )(obj).Equals( employee.IsSmoker ) )
                      && _filterVmCollection[ ( int )FilterIndexes.Rank ].Entities.Any( obj => (obj as RankViewModel)?.Number == employee.Rank.Number );

            return res;
        }
    }
}
