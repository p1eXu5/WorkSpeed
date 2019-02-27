﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class EmployeeReportViewModel : ReportViewModel
    {
        #region Fields

        private readonly ObservableCollection< ShiftGroupingViewModel > _shiftGroupingVmCollection;

        #endregion


        #region Ctor

        public EmployeeReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            _shiftGroupingVmCollection = new ObservableCollection< ShiftGroupingViewModel >();
            ShiftGroupingVmCollection = new ReadOnlyObservableCollection< ShiftGroupingViewModel >( _shiftGroupingVmCollection );

            SetupView( ShiftGroupingVmCollection );
        }

        #endregion


        #region Properties

        public ReadOnlyObservableCollection< ShiftGroupingViewModel > ShiftGroupingVmCollection { get; }

        #endregion


        #region Methods

        public override Task OnSelectedAsync ()
        {
            return LoadEmployeesAsync( null );
        }

        public override async Task UpdateAsync ()
        {
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

            await LoadEmployeesAsync( null );
        }

        private async Task LoadEmployeesAsync ( object obj )
        {
            ReportMessage = "Идёт загрузка сотрудников";
            await _reportService.LoadShiftGroupingAsync();

            if (_reportService.ShiftGroupingCollection.Any())
            {
                ReportMessage = "";

                if ( _shiftGroupingVmCollection.Any() ) {
                    _shiftGroupingVmCollection.Clear();
                }

                foreach ( var shiftGrouping in _reportService.ShiftGroupingCollection ) {

                    var vm = new ShiftGroupingViewModel( shiftGrouping, FilterVmCollection );
                    vm.PropertyChanged += OnIsModifyChanged;
                    _shiftGroupingVmCollection.Add( vm );
                    Refresh();
                }
            }
            else
            {
                ReportMessage = "Сотрудники отсутствуют. Чтобы добавить сотрудников, имортируйте их.";
            }
        }

        protected internal override void Refresh ()
        {
            foreach (var shiftGroupingViewModel in ShiftGroupingVmCollection)
            {
                shiftGroupingViewModel.Refresh();
            }

            base.Refresh();
        }

        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = ShiftGroupingVmCollection.Any( shgvm => shgvm.IsModify );
        }

        protected override bool PredicateFunc ( object o )
        {
            if ( !(o is ShiftGroupingViewModel shiftGrouping) ) { return  false; }

            return _filterVmCollection[ (int)Filters.Shift ].Entities.Any( obj => (obj as ShiftViewModel)?.Shift == shiftGrouping.Shift );

        }

        #endregion

    }
}
