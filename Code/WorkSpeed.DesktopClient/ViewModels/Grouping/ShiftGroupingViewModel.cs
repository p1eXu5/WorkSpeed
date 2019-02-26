using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class ShiftGroupingViewModel : FilteredViewModel
    {
        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping, Predicate< object > predicate )
        {
            Shift = shiftGrouping.Shift ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGroupingVmCollection cannot be null.");
            
            var appointmentGroupingVmCollection = new ObservableCollection< AppointmentGroupingViewModel >( 
                shiftGrouping.Appointments
                             .Select( a => new AppointmentGroupingViewModel( a, predicate ) ) 
            );

            AppointmentGroupingVmCollection = new ReadOnlyObservableCollection< AppointmentGroupingViewModel >( appointmentGroupingVmCollection );

            var view = SetupView( AppointmentGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Appointment.Id", ListSortDirection.Ascending ) );
        }

        public Shift Shift { get; }
        public ReadOnlyObservableCollection< AppointmentGroupingViewModel > AppointmentGroupingVmCollection { get; }

        public string Name => Shift.Name;

        protected internal override void Refresh ()
        {
            foreach ( var appointment in AppointmentGroupingVmCollection ) {
                appointment.Refresh();
            }

            base.Refresh();
        }
    }
}
