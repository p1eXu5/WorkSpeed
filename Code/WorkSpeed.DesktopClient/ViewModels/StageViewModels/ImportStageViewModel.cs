using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public abstract class ImportStageViewModel : StageViewModel
    {
        #region Fields

        private bool _isInProgress;
        private string _fileName;
        private double _progressCounter;
        

        protected CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        #endregion


        #region Ctor

        protected ImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        {
            OpenCommand = new MvvmCommand( Open );
            _message = "Выберите файл, нажав на кнопку выше.";
        }

        #endregion


        #region Properties

        

        public string FileName
        {
            get => _fileName;
            set {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public bool IsInProgress
        {
            get => _isInProgress;
            set {
                _isInProgress = value;
                OnPropertyChanged();
            }
        }

        public double ProgressCounter
        {
            get => _progressCounter;
            set {
                _progressCounter = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Commands

        public ICommand OpenCommand { get; }

        #endregion


        #region Methods

        protected abstract void Open ( object obj );            

        protected string OpenExcelFile ()
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = "Excel Files|*.xls;*.xlsx",
                RestoreDirectory = true,
            };

            if ( ofd.ShowDialog() == true ) {

                return ofd.FileName;
            }

            return null;
        }

        protected CancellationToken GetCancellationToken ()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource = new CancellationTokenSource();

            return CancellationTokenSource.Token;
        }

        protected override void Backward ( object obj )
        {
            CancellationTokenSource.Cancel();
            base.Backward( null );
        }

        #endregion
    }
}
