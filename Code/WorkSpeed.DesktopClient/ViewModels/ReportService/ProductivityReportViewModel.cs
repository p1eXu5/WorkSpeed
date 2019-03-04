using System;
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

        #region Ctor

        public ProductivityReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            _timesOperation = new Operation { Id = -1, Name = "Время" };

            SetupPeriod();

            var operationVmCollection = new ObservableCollection< OperationViewModel >( _reportService.OperationCollection.Select( o => new OperationViewModel( o ) ));
            operationVmCollection.Add( new OperationViewModel( _timesOperation ) );
            OperationVmCollection = new ReadOnlyObservableCollection< OperationViewModel >( operationVmCollection );
            Observe( _reportService.OperationCollection, operationVmCollection, o => o.Operation );

            ExtendFilters();

            SetupView( OperationVmCollection, (vsc) =>
                                                {
                                                    vsc.SortDescriptions.Add( new SortDescription( "Id", ListSortDirection.Ascending ) );
                                                    vsc.Filter = OperationPredicate;
                                                } );

            CreateEmployeeProductivityVmCollection();

            SetupView( EmployeeProductivityVmCollection, ( vsc ) =>
                                                            {
                                                                vsc.SortDescriptions.Add( new SortDescription( "PositionId", ListSortDirection.Ascending ) );
                                                                vsc.SortDescriptions.Add( new SortDescription( "AppointmentId", ListSortDirection.Ascending ) );
                                                                vsc.SortDescriptions.Add( new SortDescription( "Name", ListSortDirection.Ascending ) );
                                                                vsc.Filter = PredicateFunc;
                                                            } );




            void SetupPeriod ()
            {
                //var now = DateTime.Now;
                //var start = now.Date.Subtract( TimeSpan.FromDays( now.Day - 1 ) );
                //Period = new Period( start, now );
                var start = new DateTime( 2018, 11, 28, 8, 0, 0);
                var end = new DateTime( 2018, 11, 29, 8, 0, 0);
                Period = new Period( start, end );
            }

            void CreateEmployeeProductivityVmCollection ()
            {
                var employeeProductivityVmCollection = new ObservableCollection< EmployeeProductivityViewModel >(
                    _reportService.EmployeeProductivityCollections.Select(
                        p => new EmployeeProductivityViewModel( p, FilterVmCollection, _reportService.CategoryCollection )
                    )
                );

                EmployeeProductivityVmCollection = new ReadOnlyObservableCollection< EmployeeProductivityViewModel >( employeeProductivityVmCollection );

                (( INotifyCollectionChanged )_reportService.EmployeeProductivityCollections).CollectionChanged +=
                    ( s, e ) =>
                    {

                        if ( e.NewItems?[ 0 ] != null ) {

                            var epvm = new EmployeeProductivityViewModel( ( EmployeeProductivity )e.NewItems[ 0 ], FilterVmCollection, _reportService.CategoryCollection );
                            employeeProductivityVmCollection.Add( epvm );
                            return;
                        }

                        if ( e.OldItems?[ 0 ] != null ) {
                            employeeProductivityVmCollection.Remove( employeeProductivityVmCollection.First( vm => ReferenceEquals( vm.EmployeeProductivity, e.OldItems[ 0 ] ) ) );
                            return;
                        }

                        if ( e.NewItems?[ 0 ] == null && e.OldItems?[ 0 ] == null ) {
                            employeeProductivityVmCollection.Clear();
                        }
                    };
            }
        }

        #endregion


        #region Properties

        public ReadOnlyObservableCollection< OperationViewModel > OperationVmCollection { get; private set; }
        public ReadOnlyObservableCollection< EmployeeProductivityViewModel > EmployeeProductivityVmCollection { get; private set; }

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

            if ( EmployeeProductivityVmCollection.Any() ) {
                ReportMessage = "";
                Refresh();
            }
            else {
                ReportMessage = "Операции за указанный период отсутствуют.";
            }
        }

        
        protected internal override void Refresh ()
        {
            //foreach ( var employeeProductivityViewModel in EmployeeProductivityVmCollection ) {
            //    employeeProductivityViewModel.Refresh();
            //}

            base.Refresh();
        }
 
        protected override bool PredicateFunc ( object o )
        {
            if (!(o is EmployeeProductivityViewModel employeeProductivity)) return false;

            var res = _filterVmCollection[ ( int )FilterIndexes.IsActive ].Entities.Any( obj => (obj).Equals( employeeProductivity.EmployeeVm.IsActive ) )
                      && _filterVmCollection[ ( int )FilterIndexes.IsSmoker ].Entities.Any( obj => ((Tuple<bool?,string>)obj).Item1.Equals( employeeProductivity.EmployeeVm.IsSmoker ) )
                      && _filterVmCollection[ ( int )FilterIndexes.Rank ].Entities.Any( obj => (obj as RankViewModel)?.Number == employeeProductivity.EmployeeVm.Rank.Number );

            return res;
        }

        
        private bool OperationPredicate ( object o )
        {
            if ( !(o is OperationViewModel operation) ) return false;

            var res = _filterVmCollection[ (int)FilterIndexes.Operation ].Entities.Any( obj => (obj as Operation) == operation.Operation);

            return res;
        }

        private void ExtendFilters ()
        {
            var filter = new FilterViewModel( "Операции", FilterIndexes.Operation, OperationVmCollection.Select( o => o.Operation ), p => (( Operation )p).Name );
            _filterVmCollection.Add( filter );

            _filterVmCollection[ (int)FilterIndexes.Operation ].FilterChanged += OnPredicateChange;
        }

        protected override void OnPredicateChange ( object sender, FilterChangedEventArgs args )
        {
            if ( args.FilterIndex == FilterIndexes.Operation ) {
                ViewList[0].Refresh();
                foreach ( var vm in EmployeeProductivityVmCollection ) {
                    vm.Refresh();
                }
            }
        }

        #endregion
    }
}
