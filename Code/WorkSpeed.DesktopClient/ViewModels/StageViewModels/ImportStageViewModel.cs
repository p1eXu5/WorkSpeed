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
        protected ImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base( fastProductivityViewModel )
        {
            OpenCommand = new MvvmCommand( Open );
        }

        public ICommand OpenCommand { get; }

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
    }
}
