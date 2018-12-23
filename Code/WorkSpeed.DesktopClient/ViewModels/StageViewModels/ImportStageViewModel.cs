using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #endregion


        #region Ctor

        protected ImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base( fastProductivityViewModel )
        {
            OpenCommand = new MvvmCommand( Open );
        }

        #endregion


        #region Properties

        public bool IsInProgress
        {
            get => _isInProgress;
            set {
                _isInProgress = value;
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

        #endregion
    }
}
