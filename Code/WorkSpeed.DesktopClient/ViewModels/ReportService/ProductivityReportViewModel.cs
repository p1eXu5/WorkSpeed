using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class ProductivityReportViewModel : ReportViewModel
    {
        private Operation _timesOperation;
        private Period _period;
        private readonly ObservableCollection< OperationViewModel > _operationVmCollection;
        private IEnumerable< OperationViewModel > _filteredOperations;

        private readonly ObservableCollection< EmployeeProductivityViewModel > _employeeProductivityVmCollection;
        private IEnumerable< EmployeeProductivityViewModel > _filteredEmployeeProductivity;

        #region Ctor

        public ProductivityReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {

            SetupPeriod();

            _timesOperation = new Operation { Id = -1, Name = "Время" };

            _operationVmCollection = new ObservableCollection< OperationViewModel >( _reportService.OperationCollection.Select( o => new OperationViewModel( o ) ) ) {
                new OperationViewModel( _timesOperation )
            };
            Observe( _reportService.OperationCollection, _operationVmCollection, o => o.Operation );

            AddOperationFilter();
            OperationVmCollection = _operationVmCollection.Where( OperationPredicate ).OrderBy( o => o.Id ).ToArray();

            _employeeProductivityVmCollection = CreateEmployeeProductivityVmCollection();

            EmployeeProductivityVmCollection = _employeeProductivityVmCollection.Where( ep => IsActivePredicate( ep.EmployeeVm )
                                                                                              && PositionPredicate( ep.EmployeeVm )
                                                                                              && AppointmentPredicate( ep.EmployeeVm )
                                                                                              && ShiftPredicate( ep.EmployeeVm )
                                                                                              && RankPredicate( ep.EmployeeVm )
                                                                                              && IsSmokerPredicate( ep.EmployeeVm )
                                                                                )
                                                                                .OrderBy( ep => ep.PositionId )
                                                                                .ThenBy( ep => ep.AppointmentId )
                                                                                .ThenBy( ep => ep.Name );


            void SetupPeriod ()
            {
                //var now = DateTime.Now;
                //var start = now.Date.Subtract( TimeSpan.FromDays( now.Day - 1 ) );
                //Period = new Period( start, now );
                var start = new DateTime( 2018, 11, 28, 8, 0, 0);
                var end = new DateTime( 2018, 11, 29, 8, 0, 0);
                Period = new Period( start, end );
            }

            ObservableCollection< EmployeeProductivityViewModel > CreateEmployeeProductivityVmCollection ()
            {
                var empColl = new ObservableCollection< EmployeeProductivityViewModel >(
                    _reportService.EmployeeProductivityCollections.Select(
                        p => new EmployeeProductivityViewModel( p, FilterVmCollection, _reportService.CategoryCollection )
                    )
                );


                (( INotifyCollectionChanged )_reportService.EmployeeProductivityCollections).CollectionChanged +=
                    ( s, e ) =>
                    {

                        if ( e.NewItems?[ 0 ] != null ) {

                            var epvm = new EmployeeProductivityViewModel( ( EmployeeProductivity )e.NewItems[ 0 ], FilterVmCollection, _reportService.CategoryCollection );
                            empColl.Add( epvm );
                            return;
                        }

                        if ( e.OldItems?[ 0 ] != null ) {
                            empColl.Remove( empColl.First( vm => ReferenceEquals( vm.EmployeeProductivity, e.OldItems[ 0 ] ) ) );
                            return;
                        }

                        if ( e.NewItems?[ 0 ] == null && e.OldItems?[ 0 ] == null ) {
                            empColl.Clear();
                        }
                    };
                return empColl;
            }
        }

        #endregion


        #region Properties

        public IEnumerable< OperationViewModel > OperationVmCollection
        {
            get => _filteredOperations;
            private set {
                _filteredOperations = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable< EmployeeProductivityViewModel > EmployeeProductivityVmCollection
        {
            get => _filteredEmployeeProductivity;
            private set {
                _filteredEmployeeProductivity = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime
        {
            get => Period.Start;
            set {
                Period = new Period( value, Period.End );
                OnPropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get => Period.End;
            set {
                Period = new Period( Period.Start, value );
                OnPropertyChanged();
            }
        }

        public Period Period
        {
            get => _period;
            set {
                _period = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Methods

        public override async Task UpdateAsync ()
        {
            ReportMessage = "Идёт загрузка выработки";

            await _reportService.LoadEmployeeProductivitiesAsync( Period );

            if ( _employeeProductivityVmCollection.Any() ) {
                ReportMessage = "";
                Refresh( FilterIndexes.All );
            }
            else {
                ReportMessage = "Операции за указанный период отсутствуют.";
            }
        }
 

        protected override void OnPredicateChanged ( object sender, FilterChangedEventArgs args )
        { }


        private bool OperationPredicate ( OperationViewModel operation )
        {
            return _filterVmCollection[ (int)FilterIndexes.Operation ].Entities.Any( obj => (obj as Operation) == operation.Operation);
        }

        private void AddOperationFilter ()
        {
            var filter = new FilterViewModel( "Операции", FilterIndexes.Operation, _operationVmCollection.Select( o => o.Operation ), p => (( Operation )p).Name );
            _filterVmCollection.Add( filter );

            _filterVmCollection[ (int)FilterIndexes.Operation ].FilterChanged += OnPredicateChanged;
        }


        #endregion

        protected internal override void Refresh ( FilterIndexes filter )
        {
            if ( filter == FilterIndexes.Operation || filter == FilterIndexes.All ) {
                OperationVmCollection = _operationVmCollection.Where( OperationPredicate ).OrderBy( o => o.Id ).ToArray();

                Parallel.ForEach( EmployeeProductivityVmCollection, ( vm ) => vm.Refresh( filter ) );
            }

            if ( filter == FilterIndexes.All ) {

                EmployeeProductivityVmCollection = _employeeProductivityVmCollection.AsParallel()
                                                                                    .Where( ep => IsActivePredicate( ep.EmployeeVm )
                                                                                            && PositionPredicate( ep.EmployeeVm )
                                                                                            && AppointmentPredicate( ep.EmployeeVm )
                                                                                            && ShiftPredicate( ep.EmployeeVm )
                                                                                            && RankPredicate( ep.EmployeeVm )
                                                                                            && IsSmokerPredicate( ep.EmployeeVm )
                                                                                    )
                                                                                    .AsSequential()
                                                                                    .OrderBy( ep => ep.PositionId )
                                                                                    .ThenBy( ep => ep.AppointmentId )
                                                                                    .ThenBy( ep => ep.Name )
                                                                                    .ToArray();
            }
        }
    }
}
