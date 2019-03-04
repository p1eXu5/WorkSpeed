using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Grouping
{
    public class ShiftGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            ShiftGrouping = shiftGrouping ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGroupingVmCollection cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");
            
            CreateCollection();

            SetupView( AppointmentGroupingVmCollection, ( vsc ) =>
                                                        {
                                                            vsc.SortDescriptions.Add( new SortDescription( "Appointment.Id", ListSortDirection.Ascending ) );
                                                            vsc.Filter = PredicateFunc;
                                                        } );


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

        public ShiftGrouping ShiftGrouping { get; }
        public Shift Shift => ShiftGrouping.Shift;
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

            var res = _filterVmCollection[ (int)FilterIndexes.Appointment ].Entities.Any( obj => (obj as AppointmentViewModel)?.Appointment == appointmentGrouping.Appointment )
                      && base.PredicateFunc( o ) ;

            return res;
        }
    }
}
