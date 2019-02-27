using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService;
using WorkSpeed.DesktopClient.ViewModels.Entities;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class ShiftGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Shift = shiftGrouping.Shift ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGroupingVmCollection cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");
            
            CreateCollection();

            var view = SetupView( AppointmentGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Appointment.Id", ListSortDirection.Ascending ) );


            void CreateCollection ()
            {
                var appointmentGroupingVmCollection = new ObservableCollection< AppointmentGroupingViewModel >( 
                    shiftGrouping.Appointments
                                 .Select( a =>
                                          {
                                              var agvm = new AppointmentGroupingViewModel( a, _filterVmCollection );
                                              agvm.PropertyChanged += OnIsModifyChanged;
                                              return agvm;
                                          } ) 
                );

                AppointmentGroupingVmCollection = new ReadOnlyObservableCollection< AppointmentGroupingViewModel >( appointmentGroupingVmCollection );
            }
        }

        public Shift Shift { get; }
        public ReadOnlyObservableCollection< AppointmentGroupingViewModel > AppointmentGroupingVmCollection { get; private set; }

        public string Name => Shift.Name;

        protected internal override void Refresh ()
        {
            foreach ( var appointment in AppointmentGroupingVmCollection ) {
                appointment.Refresh();
            }

            base.Refresh();
        }

        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = AppointmentGroupingVmCollection.Any( agvm => agvm.IsModify );
        }

        protected override bool PredicateFunc ( object o )
        {
            if ( !(o is AppointmentGroupingViewModel appointmentGrouping) ) { return  false; }

            return _filterVmCollection[ (int)Filters.Appointment ].Entities.Any( obj => (obj as AppointmentViewModel)?.Appointment == appointmentGrouping.Appointment );
        }
    }
}
