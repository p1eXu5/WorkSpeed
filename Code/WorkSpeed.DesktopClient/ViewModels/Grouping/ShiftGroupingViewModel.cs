using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class ShiftGroupingViewModel 
        : FilteredViewModel
    {
        private readonly ObservableCollection< AppointmentGroupingViewModel > _appointments;

        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping, Predicate< object > predicate )
        {
            Shift = shiftGrouping.Shift ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGroupingVmCollection cannot be null."); ;
            
            _appointments = new ObservableCollection< AppointmentGroupingViewModel >( 

                shiftGrouping.Appointments
                             .Select( a => new AppointmentGroupingViewModel( a, predicate ) ) 
            );

            Appointments = new ReadOnlyObservableCollection< AppointmentGroupingViewModel >( _appointments );

            View = CollectionViewSource.GetDefaultView( Appointments );
            View.SortDescriptions.Add( new SortDescription( "Appointment.Id", ListSortDirection.Ascending ) );

            View.Filter = Predicate;
        }

        public Shift Shift { get; }
        public ReadOnlyObservableCollection< AppointmentGroupingViewModel > Appointments { get; }

        public string Name => Shift.Name;

        protected override void OnRefresh ()
        {
            foreach ( var appointment in _appointments ) {
                appointment.Refresh();
            }

            base.OnRefresh();
        }
    }
}
