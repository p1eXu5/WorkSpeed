using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class OperationViewModel : ViewModel
    {
        private readonly Operation _operation;
        private bool? _sortOrder;

        public OperationViewModel ( Operation operation )
        {
            _operation = operation;
            _sortOrder = null;
        }

        public event EventHandler< SortRequestedEventArgs > SortRequested; 

        public int Id => _operation.Id;
        public Operation Operation => _operation;
        public string Name => _operation.Name;

        public bool? SortOrder
        {
            get => _sortOrder;
            set {
                _sortOrder = value;
                OnPropertyChanged();
            }
        }

        public ICommand SortCommand => new MvvmCommand( OnSortRequested );

        private void OnSortRequested ( object o )
        {
            SortOrder so;

            if ( SortOrder == null || SortOrder == false ) {
                SortOrder = true;
                so = System.Data.SqlClient.SortOrder.Ascending;
            }
            else {
                SortOrder = false;
                so = System.Data.SqlClient.SortOrder.Descending;
            }

            SortRequested?.Invoke( this, new SortRequestedEventArgs( so ) );
        }
    }
}
