using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class OperationViewModel
    {
        private readonly Operation _operation;

        public OperationViewModel ( Operation operation )
        {
            _operation = operation;
        }

        public event EventHandler< EventArgs > SortRequested; 

        public int Id => _operation.Id;
        public Operation Operation => _operation;

        public string Name => _operation.Name;

        public ICommand SortCommand => new MvvmCommand( OnSortRequested );

        private void OnSortRequested ( object o )
        {
            SortRequested?.Invoke( this, EventArgs.Empty );
        }
    }
}
