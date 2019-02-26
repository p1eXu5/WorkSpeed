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
using WorkSpeed.DesktopClient.ViewModels.ReportService;

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

            SelectedIndex = (int)Tabs.EmployeeEditor;
            EmployeeReportVm.OnSelectedAsync();
            FilterVmCollection = EmployeeReportVm.FilterVmCollection;
        }

        #endregion


        #region Properties

        public int SelectedIndex
        {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                OnPropertyChanged();
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

        public EmployeeReportViewModel EmployeeReportVm { get; }
        public ProductivityReportViewModel ProductivityReportVm { get; }

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

        #endregion


        #region Commands

        public ICommand ImportAsyncCommand => new MvvmCommand( Import );
        public ICommand TabItemChangedCommand => new MvvmCommand( TabItemChanged );

        public string ImportStatusMessage
        {
            get => _importStatusMessage;
            set {
                _importStatusMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Methods

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

                ImportStatusMessage = "Обновление данных.";
                await EmployeeReportVm.OnSelectedAsync();
                await ProductivityReportVm.OnSelectedAsync();

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
            if ( selected == _selectedIndex ) return;

            _selectedIndex = selected;

            if ( selected == (int)Tabs.EmployeeEditor ) {
                EmployeeReportVm.OnSelectedAsync();
                FilterVmCollection = EmployeeReportVm.FilterVmCollection;
            }
            else if ( selected == ( int )Tabs.ProductivityReport ) {
                ProductivityReportVm.OnSelectedAsync();
                FilterVmCollection = ProductivityReportVm.FilterVmCollection;
            }
        }

        #endregion


        private enum Tabs
        {
            ProductivityReport = 0,
            EmployeeEditor = 1,
        }
    }
}
