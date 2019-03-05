using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using Microsoft.Win32;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class EmployeeReportViewModel : ReportViewModel
    {
        #region Fields

        private readonly ObservableCollection< ShiftGroupingViewModel > _shiftGroupingVmCollection;
        private IEnumerable< ShiftGroupingViewModel > _filteredShiftGrouping;

        #endregion


        #region Ctor

        public EmployeeReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            _shiftGroupingVmCollection = new ObservableCollection< ShiftGroupingViewModel >();
            ((INotifyCollectionChanged) _reportService.ShiftGroupingCollection).CollectionChanged += (s, e) =>
            {

                if ( e.NewItems?[ 0 ] != null ) {
                    
                    var sgvm = new ShiftGroupingViewModel( (ShiftGrouping)e.NewItems[ 0 ], FilterVmCollection );
                    sgvm.PropertyChanged += this.OnIsModifyChanged;
                    _shiftGroupingVmCollection.Add( sgvm );
                }

                if ( e.OldItems?[ 0 ] != null ) {
                    
                    var delShiftGrouping = _shiftGroupingVmCollection.First( sh => ReferenceEquals( sh.ShiftGrouping, e.OldItems[ 0 ] ) );
                    delShiftGrouping.PropertyChanged -= OnIsModifyChanged;
                    _shiftGroupingVmCollection.Remove( delShiftGrouping );
                }

                if ( e.NewItems?[ 0 ] == null && e.OldItems?[ 0 ] == null ) {
                    
                    foreach ( var shiftGroupingViewModel in _shiftGroupingVmCollection ) {
                        shiftGroupingViewModel.PropertyChanged -= OnIsModifyChanged;
                    }

                    _shiftGroupingVmCollection.Clear();
                }
            };

            ShiftGroupingVmCollection = _shiftGroupingVmCollection.Where( ShiftGroupingPredicate ).ToArray();
            //IsModify = true;
        }

        #endregion


        #region Properties

        public IEnumerable< ShiftGroupingViewModel > ShiftGroupingVmCollection
        {
            get => _filteredShiftGrouping;
            set {
                _filteredShiftGrouping = value;
                OnPropertyChanged();
            }
        }

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

        public override async Task UpdateAsync ()
        {
            if ( IsModify ) {

                var employees = from s in ShiftGroupingVmCollection
                                where s.IsModify
                                from a in s.AppointmentGroupingVmCollection
                                where a.IsModify
                                from p in a.PositionGroupingVmCollection
                                where p.IsModify
                                from e in p.EmployeeVmCollection
                                where e.IsModify
                                select e.Employee;

                _reportService.UpdateRange( employees );
                IsModify = false;
            }

            await LoadEmployeesAsync().ConfigureAwait( false );
        }

        private async Task LoadEmployeesAsync ()
        {
            ReportMessage = "Идёт загрузка сотрудников";

            await _reportService.LoadShiftGroupingAsync();

            if ( _shiftGroupingVmCollection.Any() ) {
                ReportMessage = "";
                Refresh();
            }
            else {
                ReportMessage = "Сотрудники отсутствуют. Чтобы добавить сотрудников, имортируйте их.";
            }

        }

        protected internal override void Refresh ()
        {
            foreach ( var shiftGroupingViewModel in _shiftGroupingVmCollection ) {
                shiftGroupingViewModel.Refresh();
            }

            ShiftGroupingVmCollection = _shiftGroupingVmCollection.Where( ShiftGroupingPredicate ).ToArray();
        }

        protected void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( "IsModify" ) ) return;

            IsModify = ShiftGroupingVmCollection.Any( shgvm => shgvm.IsModify );
        }

        private bool ShiftGroupingPredicate ( ShiftGroupingViewModel shiftGrouping )
            => _filterVmCollection[ (int)FilterIndexes.Shift ].Entities.Any( obj => (obj as Shift) == shiftGrouping.Shift)
                && shiftGrouping.AppointmentGroupingVmCollection.Any();

        #endregion

    }
}
