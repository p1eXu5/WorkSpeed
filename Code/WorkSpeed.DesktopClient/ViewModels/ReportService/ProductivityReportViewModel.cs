using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;
using WorkSpeed.DesktopClient.ViewModels.Productivity;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class ProductivityReportViewModel : ReportViewModel
    {
        protected const int OPERATION = 6;

        private ObservableCollection< EmployeeProductivityViewModel > _employeeProductivityVmCollection;
        private readonly ICollectionView _operationView;

        #region Ctor

        public ProductivityReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            CreateEmployeeProductivityVmCollection();
            ExtendFilters();

            var operationVmCollection = new ObservableCollection< OperationViewModel >( _reportService.OperationCollection.Select( o => new OperationViewModel( o ) ));
            OperationVmCollection = new ReadOnlyObservableCollection< OperationViewModel >( operationVmCollection );
            Observe( _reportService.OperationCollection, operationVmCollection, o => o.Operation );
            
            View = SetupView( EmployeeProductivityVmCollection, EmployeeProductivityPredicate );

            View.SortDescriptions.Add( new SortDescription( "PositionId", ListSortDirection.Ascending ) );
            View.SortDescriptions.Add( new SortDescription( "AppointmentId", ListSortDirection.Ascending ) );
            View.SortDescriptions.Add( new SortDescription( "Name", ListSortDirection.Ascending ) );

            _operationView = SetupView( OperationVmCollection, OperationPredicate );
        }

        private void CreateEmployeeProductivityVmCollection ()
        {
            _employeeProductivityVmCollection = new ObservableCollection< EmployeeProductivityViewModel >(
                _reportService.EmployeeProductivityCollections.Select(
                    p => new EmployeeProductivityViewModel( p, _reportService.OperationCollection, _reportService.CategoryCollection, ProductivityPredicate )
                )
            );

            EmployeeProductivityVmCollection = new ReadOnlyObservableCollection< EmployeeProductivityViewModel >( _employeeProductivityVmCollection );

            (( INotifyCollectionChanged )_reportService.EmployeeProductivityCollections).CollectionChanged +=
                ( s, e ) =>
                {
                    if ( e.NewItems[ 0 ] != null ) {
                        _employeeProductivityVmCollection.Add(
                            new EmployeeProductivityViewModel(
                                ( EmployeeProductivityCollection )e.NewItems[ 0 ],
                                _reportService.OperationCollection,
                                _reportService.CategoryCollection,
                                ProductivityPredicate
                            )
                        );
                    }

                    if ( e.OldItems[ 0 ] != null ) {
                        _employeeProductivityVmCollection.Remove( _employeeProductivityVmCollection.First( vm => ReferenceEquals( vm.EmployeeProductivityCollection, e.OldItems[ 0 ] ) ) );
                    }

                    if ( e.NewItems[ 0 ] == null && e.OldItems[ 0 ] == null ) {
                        _employeeProductivityVmCollection.Clear();
                    }
                };
        }

        #endregion


        #region Properties

        public ReadOnlyObservableCollection< OperationViewModel > OperationVmCollection { get; private set; }
        public ReadOnlyObservableCollection< EmployeeProductivityViewModel > EmployeeProductivityVmCollection { get; private set; }

        public Period Period
        {
            get => _reportService.Period;
            set {
                _reportService.SetPeriodAsync( value );
                OnPropertyChanged();
            }
        }

        #endregion



        public override void OnSelectedAsync ()
        {
            throw new NotImplementedException();
        }

        private async void LoadEmployeeProductivityAsync ()
        {
            await _reportService.LoadEmployeeProductivitiesAsync();
        }

        private void ExtendFilters ()
        {
            var filter = new EntityFilterViewModel< object >( "Операции", _reportService.OperationCollection, p => (( Operation )p).Name );
            _filterVmCollection.Add( filter );

            ((INotifyCollectionChanged)_filterVmCollection[ OPERATION ].Entities).CollectionChanged += OnPredicateChange;
        }
 
        private bool EmployeeProductivityPredicate ( object o )
        {
            if (!(o is EmployeeProductivityViewModel employeeProductivityViewModel)) return false;

            return base.EmployeePredicate( employeeProductivityViewModel.EmployeeVm );
        }

        private bool OperationPredicate ( object o )
        {
            if (!(o is OperationViewModel operation)) return false;
            return _filterVmCollection[ OPERATION ].Entities.Any( obj => (obj as Operation) == operation.Operation);
        }

        private bool ProductivityPredicate ( object o )
        {
            if ( !( o is ProductivityViewModel productivityVm ) ) return false;
            return _filterVmCollection[ OPERATION ].Entities.Any( obj => (obj as Operation) == productivityVm.Operation);
        }

        protected override void OnRefresh ()
        {
            base.OnRefresh();
            _operationView.Refresh();
            foreach ( var employeeProductivityViewModel in View.SourceCollection.Cast< EmployeeProductivityViewModel >() ) {
                employeeProductivityViewModel.Refresh();
            }
        }
    }
}
