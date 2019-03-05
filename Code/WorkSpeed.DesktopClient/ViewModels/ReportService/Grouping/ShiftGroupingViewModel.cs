using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Grouping
{
    public class ShiftGroupingViewModel : FilteredViewModel
    {
        #region Fields

        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;
        private readonly ObservableCollection< AppointmentGroupingViewModel > _appointmentGroupingVmCollection;
        private IEnumerable< AppointmentGroupingViewModel > _filteredAppointmentGrouping;

        #endregion


        #region Ctor

        public ShiftGroupingViewModel ( ShiftGrouping shiftGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            ShiftGrouping = shiftGrouping ?? throw new ArgumentNullException(nameof(shiftGrouping), @"ShiftGroupingVmCollection cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");
            
            _appointmentGroupingVmCollection = new ObservableCollection< AppointmentGroupingViewModel >( 
                shiftGrouping.Appointments
                                .Select( a =>
                                        {
                                            var agvm = new AppointmentGroupingViewModel( a, _filterVmCollection );
                                            agvm.PropertyChanged += OnIsModifyChanged;
                                            return agvm;
                                        } ) 
            );

            AppointmentGroupingVmCollection = _appointmentGroupingVmCollection.Where( AppointmentGroupingPredicate ).ToArray();
        }

        #endregion


        #region Properties

        public ShiftGrouping ShiftGrouping { get; }
        public Shift Shift => ShiftGrouping.Shift;

        public IEnumerable< AppointmentGroupingViewModel > AppointmentGroupingVmCollection
        {
            get => _filteredAppointmentGrouping;
            private set {
                _filteredAppointmentGrouping = value;
                OnPropertyChanged();
            }
        }

        public string Name => Shift.Name;

        private bool _isModify;
        public bool IsModify
        {
            get => _isModify;
            set {
                _isModify = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Methods

        protected internal override void Refresh ( FilterIndexes filter )
        {
            foreach ( var appointment in _appointmentGroupingVmCollection ) {
                appointment.Refresh( filter );
            }

            AppointmentGroupingVmCollection = _appointmentGroupingVmCollection.Where( AppointmentGroupingPredicate ).ToArray();
        }

        protected void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( "IsModify" ) ) return;

            IsModify = AppointmentGroupingVmCollection.Any( agvm => agvm.IsModify );
        }

        private bool AppointmentGroupingPredicate ( AppointmentGroupingViewModel appointmentGrouping )
            => _filterVmCollection[ (int)FilterIndexes.Appointment ].Entities.Any( obj => (obj as Appointment) == appointmentGrouping.Appointment)
                && appointmentGrouping.PositionGroupingVmCollection.Any();

        #endregion
    }
}
