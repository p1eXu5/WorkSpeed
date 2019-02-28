using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class ProductivityReportViewModel : ReportViewModel
    {
        #region Ctor

        public ProductivityReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            ExtendFilters();

            var operationVmCollection = new ObservableCollection< OperationViewModel >( _reportService.OperationCollection.Select( o => new OperationViewModel( o ) ));
            OperationVmCollection = new ReadOnlyObservableCollection< OperationViewModel >( operationVmCollection );
            Observe( _reportService.OperationCollection, operationVmCollection, o => o.Operation );

            SetupView( OperationVmCollection, OperationPredicate );

            CreateEmployeeProductivityVmCollection();

            var view = SetupView( EmployeeProductivityVmCollection );

            view.SortDescriptions.Add( new SortDescription( "PositionId", ListSortDirection.Ascending ) );
            view.SortDescriptions.Add( new SortDescription( "AppointmentId", ListSortDirection.Ascending ) );
            view.SortDescriptions.Add( new SortDescription( "Name", ListSortDirection.Ascending ) );

            
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
                            employeeProductivityVmCollection.Add(
                                new EmployeeProductivityViewModel(
                                    ( EmployeeProductivity )e.NewItems[ 0 ],
                                    FilterVmCollection,
                                    _reportService.CategoryCollection
                                )
                            );
                        }

                        if ( e.OldItems?[ 0 ] != null ) {
                            employeeProductivityVmCollection.Remove( employeeProductivityVmCollection.First( vm => ReferenceEquals( vm.EmployeeProductivity, e.OldItems[ 0 ] ) ) );
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
            get => _reportService.Period;
            set {
                _reportService.SetPeriodAsync( value ).FireAndForgetSafeAsync();
                OnPropertyChanged();
            }
        }

        #endregion


        #region Methods

        public override async Task UpdateAsync ()
        {
            await LoadEmployeeProductivityAsync().ConfigureAwait( false );
        }

        
        protected internal override void Refresh ()
        {
            foreach ( var employeeProductivityViewModel in EmployeeProductivityVmCollection ) {
                employeeProductivityViewModel.Refresh();
            }

            base.Refresh();
        }
 
        protected override bool PredicateFunc ( object o )
        {
            if (!(o is EmployeeProductivityViewModel employeeProductivity)) return false;

            var res = _filterVmCollection[ ( int )FilterIndexes.IsActive ].Entities.Any( obj => (obj).Equals( employeeProductivity.EmployeeVm.IsActive ) )
                      && _filterVmCollection[ ( int )FilterIndexes.IsSmoker ].Entities.Any( obj => (obj).Equals( employeeProductivity.EmployeeVm.IsSmoker ) )
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
            var filter = new FilterViewModel( "Операции", _reportService.OperationCollection, p => (( Operation )p).Name );
            _filterVmCollection.Add( filter );

            _filterVmCollection[ (int)FilterIndexes.Operation ].FilterChanged += OnPredicateChange;
        }

        private async Task LoadEmployeeProductivityAsync ()
        {
            ReportMessage = "Идёт загрузка выработки";

            await _reportService.LoadEmployeeProductivitiesAsync();

            if ( EmployeeProductivityVmCollection.Any() ) {
                ReportMessage = "";
                Refresh();
            }
            else {
                ReportMessage = "Операции за указанный период отсутствуют.";
            }
        }

        #endregion
    }
}
