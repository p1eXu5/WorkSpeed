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
    public class AppointmentGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

            CreateCollection();

            var view = SetupView( PositionGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Position.Id", ListSortDirection.Ascending ) );


            void CreateCollection ()
            {
                var positionGroupingVmCollection = new ObservableCollection< PositionGroupingViewModel >( 
                    appointmentGrouping.PositionGrouping
                                       .Select( p =>
                                                {
                                                    var pgvm = new PositionGroupingViewModel( p, _filterVmCollection );
                                                    pgvm.PropertyChanged += OnIsModifyChanged;
                                                    return pgvm;
                                                } ) 
                );
                PositionGroupingVmCollection = new ReadOnlyObservableCollection< PositionGroupingViewModel >( positionGroupingVmCollection );
            }
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionGroupingViewModel > PositionGroupingVmCollection { get; private set; }

        public string Name => Appointment.InnerName;

        protected internal override void Refresh ()
        {
            foreach ( var position in PositionGroupingVmCollection ) {
                position.Refresh();
            }

            base.Refresh();
        }

        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = PositionGroupingVmCollection.Any( pgvm => pgvm.IsModify );
        }

        protected override bool PredicateFunc ( object o )
        {
            if ( !(o is PositionGroupingViewModel positionGrouping) ) { return  false; }

            var res = _filterVmCollection[ (int)FilterIndexes.Position ].Entities.Any( obj => (obj as PositionViewModel)?.Position == positionGrouping.Position )
                      && base.PredicateFunc( o );

            return res;
        }
    }
}
