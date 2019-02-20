using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using Microsoft.Win32;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Dialogs;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.Grouping;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IImportService _importService;
        private readonly IDialogRepository _dialogRepository;
        private readonly ReportService _reportService;

        private readonly ObservableCollection< ShiftViewModel > _shifts;
        private readonly ObservableCollection< ShortBreakScheduleViewModel > _shortBreakSchedules;

        private readonly ObservableCollection< ShiftGroupingViewModel > _shiftGrouping;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IProgress< (int, string) > _progress;

        private bool _isImporting;
        private int? _importPercentage;
        private string _importStatusMessage;
        private string _employeesStatusMessage;

        private int _selectedTab;

        public MainViewModel ( IImportService importService, ReportService reportService, IDialogRepository dialogRepository )
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService), @"IImportService cannot be null.");
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService), @"ReportService cannot be null.");
            _dialogRepository = dialogRepository;
            _progress = new Progress< (int code, string message) >( t => ProgressReport( t.code, t.message ) );

            _shifts = new ObservableCollection< ShiftViewModel >( _reportService.Shifts.Select( sh => new ShiftViewModel( sh ) ) );
            ShiftVms = new ReadOnlyObservableCollection< ShiftViewModel >( _shifts );
            Observe( _reportService.Shifts, _shifts, sh => sh.Shift );

            _shortBreakSchedules = new ObservableCollection< ShortBreakScheduleViewModel >( _reportService.ShortBreakSchedules.Select( sbs => new ShortBreakScheduleViewModel( sbs ) ));
            ShortBreakScheduleVms = new ReadOnlyObservableCollection< ShortBreakScheduleViewModel >( _shortBreakSchedules );
            Observe( _reportService.ShortBreakSchedules, _shortBreakSchedules, sbs => sbs.ShortBreakSchedule  );

            AppointmentVms = _reportService.Appointments.Select( a => new AppointmentViewModel( a ) ).ToArray();
            Ranks = _reportService.Ranks;
            PositionVms = _reportService.Positions.Select( p => new PositionViewModel( p ) ).ToArray();

            _shiftGrouping = new ObservableCollection< ShiftGroupingViewModel >();
            ShiftGroupingVms = new ReadOnlyObservableCollection< ShiftGroupingViewModel >( _shiftGrouping );

            var view = CollectionViewSource.GetDefaultView( ShiftGroupingVms );
            view.SortDescriptions.Add( new SortDescription( "Shift.Id", ListSortDirection.Ascending ) );
        }

        public bool IsImporting
        {
            get => _isImporting;
            set {
                _isImporting = value;
                OnPropertyChanged();
            }
        }

        public int? ImportPercentage
        {
            get => _importPercentage;
            set {
                _importPercentage = value;
                OnPropertyChanged();
            }
        }

        public string ImportStatusMessage
        {
            get => _importStatusMessage;
            set {
                _importStatusMessage = value;
                OnPropertyChanged();
            }
        }

        public string EmployeesStatusMessage
        {
            get => _employeesStatusMessage;
            set {
                _employeesStatusMessage = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable< AppointmentViewModel > AppointmentVms { get; }
        public IEnumerable< Rank > Ranks { get; }
        public IEnumerable< PositionViewModel > PositionVms { get; }

        public ReadOnlyObservableCollection< ShiftViewModel > ShiftVms { get; }
        public ReadOnlyObservableCollection< ShortBreakScheduleViewModel > ShortBreakScheduleVms { get; }

        public ReadOnlyObservableCollection< ShiftGroupingViewModel > ShiftGroupingVms { get; }

        public ICommand ImportAsyncCommand => new MvvmCommand( Import );
        public ICommand TabItemChangedCommand => new MvvmCommand( TabItemChanged );

        private async void Import ( object obj )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = "Excel Files|*.xls;*.xlsx",
                RestoreDirectory = true,
            };

            if ( true == ofd.ShowDialog() ) {

                IsImporting = true;

                var token = _cancellationTokenSource.Token;
                await _importService.ImportFromXlsxAsync( ofd.FileName, _progress, token );

                IsImporting = false;
            }
        }

        private void ProgressReport ( int code, string message )
        {
            if ( code == -1 ) {
                ShowError( message );
                return;
            }

            if ( code > 0 && code <= 100 ) {
                
                ImportPercentage = code;
                ImportStatusMessage = message;
            }
        }

        private void ShowError ( string message )
        {
            var view = _dialogRepository.GetView( new ErrorViewModel( message ) );
            view?.ShowDialog();
        }

        private void TabItemChanged ( object obj )
        {
            int selected = (int)obj;
            if ( selected == _selectedTab ) return;

            _selectedTab = selected;
            
            if ( selected == 1 ) { LoadEmployees( obj ); }
        }

        private void LoadEmployees ( object obj )
        {
            EmployeesStatusMessage = "Идёт загрузка сотрудников";
            _reportService.LoadEmployees();

            if (_reportService.ShiftGrouping.Any())
            {

                EmployeesStatusMessage = "";

                if (_shiftGrouping.Any()) { _shiftGrouping.Clear(); }

                foreach (var shiftGrouping in _reportService.ShiftGrouping)
                {
                    _shiftGrouping.Add(new ShiftGroupingViewModel(shiftGrouping));
                }
            }
            else
            {
                EmployeesStatusMessage = "Сотрудники отсутствуют. Чтобы добавить сотрудников, имортируйте их.";
            }
        }
    }
}
