using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class EmployeeReportViewModel : ReportViewModel
    {
        private readonly ObservableCollection< ShiftGroupingViewModel > _shiftGroupingVmCollection;


        public EmployeeReportViewModel ( IReportService reportService, IDialogRepository dialogRepository )
            : base( reportService, dialogRepository )
        {
            _shiftGroupingVmCollection = new ObservableCollection< ShiftGroupingViewModel >();
            ShiftGroupingVmCollection = new ReadOnlyObservableCollection< ShiftGroupingViewModel >( _shiftGroupingVmCollection );

            var view = CollectionViewSource.GetDefaultView( ShiftGroupingVmCollection );
            view.SortDescriptions.Add( new SortDescription( "Shift.Id", ListSortDirection.Ascending ) );
        }
        

        public ReadOnlyObservableCollection< ShiftGroupingViewModel > ShiftGroupingVmCollection { get; }


        private async Task LoadEmployeesAsync ( object obj )
        {
            ReportMessage = "Идёт загрузка сотрудников";
            await _reportService.LoadEmployeesAsync();

            if (_reportService.ShiftGroupingCollection.Any())
            {

                ReportMessage = "";

                if ( _shiftGroupingVmCollection.Any() ) {
                    _shiftGroupingVmCollection.Clear();
                }

                foreach ( var shiftGrouping in _reportService.ShiftGroupingCollection )
                {
                    _shiftGroupingVmCollection.Add( new ShiftGroupingViewModel( shiftGrouping ) );
                }
            }
            else
            {
                ReportMessage = "Сотрудники отсутствуют. Чтобы добавить сотрудников, имортируйте их.";
            }
        }

        public override async void OnSelectedAsync ()
        {
            await LoadEmployeesAsync( null );
        }
    }
}
