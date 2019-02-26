using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class AppointmentGroupingViewModel : FilteredViewModel
    {
        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping, Predicate< object > predicate )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );

            var positionGroupingVmCollection = new ObservableCollection< PositionGroupingViewModel >( 
                appointmentGrouping.PositionGrouping
                                   .Select( p => new PositionGroupingViewModel( p, predicate ) ) 
            );
            PositionGroupingVmCollection = new ReadOnlyObservableCollection< PositionGroupingViewModel >( positionGroupingVmCollection );

            var view = SetupView( PositionGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Position.Id", ListSortDirection.Ascending ) );
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionGroupingViewModel > PositionGroupingVmCollection { get; }

        public string Name => Appointment.InnerName;

        protected internal override void Refresh ()
        {
            foreach ( var position in PositionGroupingVmCollection ) {
                position.Refresh();
            }

            base.Refresh();
        }
    }
}
