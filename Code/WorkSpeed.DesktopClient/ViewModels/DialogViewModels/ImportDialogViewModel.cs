using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.DialogViewModels
{
    public class ImportDialogViewModel< TImportModel > : ViewModel, IDialogCloseRequested
        where TImportModel : ImportModel
    {
        private readonly IWarehouse _warehouse;
        private bool _isFileProcessing;

        public ImportDialogViewModel( IWarehouse warehouse )
        {
            _warehouse = warehouse ?? throw new ArgumentNullException();
        }

        public ICommand OpenFileCommand => new MvvmCommand( OpenFileAsync, IsFileProcess );

        public ICommand OkCommand => new MvvmCommand( (o) => { } );
        public ICommand CanselCommand => new MvvmCommand( (o) => { } );

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        private async void OpenFileAsync ( object obj )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyComputer),
                Filter = "Excel Files|*.xls;*.xlsx",
                RestoreDirectory = true,
            };

            if ( true == ofd.ShowDialog() ) {

                _isFileProcessing = true;
                ((MvvmCommand)OpenFileCommand).RaiseCanExecuteChanged();

                await _warehouse.ImportAsync< TImportModel >( ofd.FileName );

                _isFileProcessing = false;
                ((MvvmCommand)OpenFileCommand).RaiseCanExecuteChanged();
            }
        }

        private bool IsFileProcess(object obj) => !_isFileProcessing;
    }
}
