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

        private IProgress< (int, string) > _progress;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        private bool _isImporting;
        private int? _importPercentage;
        private string _importStatusMessage;
        
        private int _selectedTab;


        public MainViewModel ( IImportService importService, ReportService reportService, IDialogRepository dialogRepository )
        {
            _importService = importService ?? throw new ArgumentNullException(nameof(importService), @"IImportService cannot be null.");
            _dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository), @"IDialogRepository cannot be null.");

           _progress = new Progress< (int code, string message) >( t => ProgressReport( t.code, t.message ) );

            EmployeeReportViewModel = new EmployeeReportViewModel( reportService, dialogRepository );
            ProductivityReportViewModel = new ProductivityReportViewModel( reportService, dialogRepository );

            _selectedTab = (int)Tabs.EmployeeEditor;
        }


        public IReportViewModel EmployeeReportViewModel { get; }
        public IReportViewModel ProductivityReportViewModel { get; }


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

            if ( selected == 1 ) {
                EmployeeReportViewModel.OnSelectedAsync();
            }
        }

        private enum Tabs
        {
            EmployeeEditor = 0,
            ProductivityReport,
        }
    }
}
