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
    public class PositionGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;
        private readonly ObservableCollection< EmployeeViewModel > _employeeVmCollection;
        private IEnumerable< EmployeeViewModel > _filteredEmployees;

        public PositionGroupingViewModel ( PositionGrouping positionGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Position = positionGrouping.Position ?? throw new ArgumentNullException(nameof(positionGrouping), @"PositionGrouping cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

 
            _employeeVmCollection = new ObservableCollection< EmployeeViewModel >( 
                positionGrouping.Employees
                                .Select( e =>
                                        {
                                            var evm = new EmployeeViewModel( e );
                                            evm.PropertyChanged += OnIsModifyChanged;
                                            return evm;
                                        } ) 
            );

            EmployeeVmCollection = _employeeVmCollection.Where( ep => IsActivePredicate( ep )
                                                                      && PositionPredicate( ep )
                                                                      && AppointmentPredicate( ep )
                                                                      && ShiftPredicate( ep )
                                                                      && RankPredicate( ep )
                                                                      && IsSmokerPredicate( ep )
                                                        )
                                                        .OrderBy( ep => ep.SecondName );

        }

        public Position Position { get; }
        
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

        public IEnumerable< EmployeeViewModel > EmployeeVmCollection
        {
            get => _filteredEmployees;
            private set {
                _filteredEmployees = value;
                OnPropertyChanged();
            }
        }


        protected internal override void Refresh ()
        {
            EmployeeVmCollection = _employeeVmCollection.Where( ep => IsActivePredicate( ep )
                                                                      && PositionPredicate( ep )
                                                                      && AppointmentPredicate( ep )
                                                                      && ShiftPredicate( ep )
                                                                      && RankPredicate( ep )
                                                                      && IsSmokerPredicate( ep )
                                                        )
                                                        .OrderBy( ep => ep.SecondName );
        }

        private void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( "IsModify" ) ) return;

            IsModify = EmployeeVmCollection.Any( evm => evm.IsModify );
        }


        protected bool IsActivePredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.IsActive ].Entities.Any( obj => (obj).Equals( employee.IsActive ) );

        protected bool PositionPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Position ].Entities.Any( obj => (obj as Position) == employee.Position);

        protected bool AppointmentPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Appointment ].Entities.Any( obj => (obj as Appointment) == employee.Appointment);

        protected bool ShiftPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Shift ].Entities.Any( obj => (obj as Shift) == employee.Shift);

        protected bool RankPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.Rank ].Entities.Any( obj => (( Rank )obj).Number == employee.Rank.Number);

        protected bool IsSmokerPredicate ( EmployeeViewModel employee )
            => _filterVmCollection[ (int)FilterIndexes.IsSmoker ].Entities.Any( obj => ((Tuple<bool?,string>)obj).Item1.Equals( employee.IsSmoker ) );
    }
}
