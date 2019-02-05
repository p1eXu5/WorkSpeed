﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using Microsoft.Win32;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.BusinessContexts.Contracts;
using WorkSpeed.Productivity;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IImportService _importService;

        protected CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        protected IProgress< (string, int) > Progress;


        public MainViewModel ( IImportService importService )
        {
            _importService = importService;
        }

        public ICommand ImportAsyncCommand => new MvvmCommand( ImportAsync );

        private async void ImportAsync ( object obj )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = "Excel Files|*.xls;*.xlsx",
                RestoreDirectory = true,
            };

            if ( true == ofd.ShowDialog() ) {

                await _warehouse.ImportAsync( ofd.FileName );
            }
        }
    }
}
