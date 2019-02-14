using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using Microsoft.Win32;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.DesktopClient.ViewModels.Dialogs;
using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;
using WorkSpeed.Productivity;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IImportService _importService;
        private readonly IDialogRepository _dialogRepository;
        private readonly ReportService _reportService;
        private readonly ObservableCollection< ShiftViewModel > _shifts;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IProgress< (int, string) > _progress;

        private bool _isImporting;
        private int? _importPercentage;
        private string _importMessage;

        public MainViewModel ( IImportService importService, ReportService reportService, IDialogRepository dialogRepository )
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService), @"IImportService cannot be null.");
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService), @"ReportService cannot be null.");
            _dialogRepository = dialogRepository;
            _progress = new Progress< (int code, string message) >( t => ProgressReport( t.code, t.message ) );

            _shifts = new ObservableCollection< ShiftViewModel >();
            Shifts = new ReadOnlyObservableCollection< ShiftViewModel >( _shifts );

            //Observe( _reportService.Shifts, _shifts, vm => vm.Shift );
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

        public string ImportMessage
        {
            get => _importMessage;
            set {
                _importMessage = value;
                OnPropertyChanged();
            }
        }

        public ReadOnlyObservableCollection< ShiftViewModel > Shifts { get; set; }

        public ICommand ImportAsyncCommand => new MvvmCommand( Import );
        public ICommand LoadEmployeesCommand => new MvvmCommand( LoadEmployees );

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
                ImportMessage = message;
            }
        }

        private void ShowError ( string message )
        {
            var view = _dialogRepository.GetView( new ErrorViewModel( message ) );
            view?.ShowDialog();
        }

        private void LoadEmployees ( object obj )
        {
            Debug.WriteLine( "TabItem loaded" );
            _reportService.LoadEmployees();
        }
    }
}
