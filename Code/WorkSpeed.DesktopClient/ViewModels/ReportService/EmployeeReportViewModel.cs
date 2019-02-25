using System;
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
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class EmployeeReportViewModel : ReportViewModel
    {
        private readonly ObservableCollection< ShiftGroupingViewModel > _shiftGroupingVmCollection;
        

        public EmployeeReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            _shiftGroupingVmCollection = new ObservableCollection< ShiftGroupingViewModel >();
            ShiftGroupingVmCollection = new ReadOnlyObservableCollection< ShiftGroupingViewModel >( _shiftGroupingVmCollection );

            SetupView( ShiftGroupingVmCollection );
        }

        
        public ReadOnlyObservableCollection< ShiftGroupingViewModel > ShiftGroupingVmCollection { get; }


        #region Methods


        public override async void OnSelectedAsync ()
        {
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

                    var vm = new ShiftGroupingViewModel( shiftGrouping, EmployeePredicate );
                    _shiftGroupingVmCollection.Add( vm );
                }
            }
            else
            {
                ReportMessage = "Сотрудники отсутствуют. Чтобы добавить сотрудников, имортируйте их.";
            }
        }

        protected override void OnRefresh ()
        {
            foreach ( var shiftGroupingViewModel in ShiftGroupingVmCollection ) {
                shiftGroupingViewModel.Refresh();
            }

            base.OnRefresh();
        }


        #endregion

    }
}
