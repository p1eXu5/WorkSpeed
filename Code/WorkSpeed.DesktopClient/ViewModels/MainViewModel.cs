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
using WorkSpeed.DesktopClient.ViewModels.ReportService;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Fields

        private readonly IImportService _importService;
        private readonly IDialogRepository _dialogRepository;

        private IProgress< (int, string) > _progress;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        private bool _isImporting;
        private int? _importPercentage;
        private string _importStatusMessage;
        
        private int _selectedIndex;

        private ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        #endregion


        #region Ctor

        public MainViewModel ( IImportService importService, IReportService reportService, IDialogRepository dialogRepository )
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService), @"IImportService cannot be null.");
            _dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository), @"IDialogRepository cannot be null.");

           _progress = new Progress< (int code, string message) >( t => ProgressReport( t.code, t.message ) );

            EmployeeReportVm = new EmployeeReportViewModel( reportService, dialogRepository );
            ProductivityReportVm = new ProductivityReportViewModel( reportService, dialogRepository );
            
        }

        #endregion


        #region Properties

        public int SelectedIndex
        {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                OnPropertyChanged();
                UpdateAsyncCommand.Execute( null );
            }
        }

        public ReadOnlyObservableCollection< FilterViewModel > FilterVmCollection
        {
            get => _filterVmCollection;
            set {
                _filterVmCollection = value;
                OnPropertyChanged();
            }
        }

        public ReportViewModel EmployeeReportVm { get; }
        public ReportViewModel ProductivityReportVm { get; }

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

        #endregion


        #region Commands

        public ICommand LoadedCommand => new MvvmCommand( OnWindowLoaded );
        public IAsyncCommand ImportAsyncCommand => new MvvmAsyncCommand( ImportAsync );
        public IAsyncCommand UpdateAsyncCommand => new MvvmAsyncCommand( UpdateAsync );

        #endregion


        #region Methods

        private void OnWindowLoaded ( object o )
        {
            SelectedIndex = (int)Tabs.ProductivityReport;
        }

        private async Task ImportAsync ( object obj )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = "Excel Files|*.xls;*.xlsx",
                RestoreDirectory = true,
            };

            if ( true == ofd.ShowDialog() ) {

                IsImporting = true;

                try {
                    await ImportAsync( ofd.FileName );
                }
                finally {
                    IsImporting = false;
                }
            }
        }

        private async Task ImportAsync ( string fileName )
        {
            var token = _cancellationTokenSource.Token;
            await _importService.ImportFromXlsxAsync( fileName, _progress, token );
            await ((MvvmAsyncCommand)UpdateAsyncCommand).ExecuteAsync().ConfigureAwait( false );
        }

        private async Task UpdateAsync ( object o )
        {
            switch ( SelectedIndex ) {

                case (int)Tabs.EmployeeEditor:
                    FilterVmCollection = EmployeeReportVm.FilterVmCollection;
                    await EmployeeReportVm.UpdateAsync().ConfigureAwait( false );
                    break;

                case (int)Tabs.ProductivityReport:
                    FilterVmCollection = ProductivityReportVm.FilterVmCollection;
                    await ProductivityReportVm.UpdateAsync().ConfigureAwait( false );
                    break;
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

        #endregion


        protected enum Tabs
        {
            ProductivityReport = 0,
            EmployeeEditor = 1,
        }
    }
}
