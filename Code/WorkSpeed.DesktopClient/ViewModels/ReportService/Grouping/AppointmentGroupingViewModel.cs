using System;
using System.Collections.Generic;
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
        #region Fields

        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;
        private readonly ObservableCollection< PositionGroupingViewModel > _positionGroupingVmCollection;
        private IEnumerable< PositionGroupingViewModel > _filteredPositionGrouping;

        #endregion


        #region Ctor

        public AppointmentGroupingViewModel ( AppointmentGrouping appointmentGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Appointment = appointmentGrouping.Appointment ?? throw new ArgumentNullException( nameof( appointmentGrouping ), @"AppointmentGrouping cannot be null." );
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");


            _positionGroupingVmCollection = new ObservableCollection< PositionGroupingViewModel >( 
                appointmentGrouping.PositionGrouping
                                    .Select( p =>
                                            {
                                                var pgvm = new PositionGroupingViewModel( p, _filterVmCollection );
                                                pgvm.PropertyChanged += OnIsModifyChanged;
                                                return pgvm;
                                            } ) 
            );
            PositionGroupingVmCollection = _positionGroupingVmCollection.Where( PositionGroupingPredicate ).ToArray();
        }

        #endregion


        #region Properties

        public Appointment Appointment { get;}

        public IEnumerable< PositionGroupingViewModel > PositionGroupingVmCollection
        {
            get => _filteredPositionGrouping;
            private set {
                _filteredPositionGrouping = value;
                OnPropertyChanged();
            }
        }

        public string Name => Appointment.InnerName;

        private bool _isModify;
        public bool IsModify
        {
            get => _isModify;
            set {
                if ( _isModify != value ) {
                    _isModify = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion


        #region Methods

        protected internal override void Refresh ()
        {
            foreach ( var position in _positionGroupingVmCollection ) {
                position.Refresh();
            }

            PositionGroupingVmCollection = _positionGroupingVmCollection.Where( PositionGroupingPredicate ).ToArray();
        }

        protected void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( "IsModify" ) ) return;

            IsModify = PositionGroupingVmCollection.Any( pgvm => pgvm.IsModify );
        }

        private bool PositionGroupingPredicate ( PositionGroupingViewModel positionGrouping )
            => _filterVmCollection[ (int)FilterIndexes.Position ].Entities.Any( obj => (obj as Position) == positionGrouping.Position)
                && positionGrouping.EmployeeVmCollection.Any();

        #endregion
    }
}
