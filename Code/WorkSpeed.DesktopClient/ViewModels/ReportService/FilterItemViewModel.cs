using System;
using Agbm.Wpf.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class FilterItemViewModel< T > : ViewModel
    {
        private bool _isChecked;

        public FilterItemViewModel ( T entity, Func< T,string > caption )
        {
            Entity = entity;
            Caption = caption.Invoke( entity );
        }

        public T Entity { get; }
        public string Caption { get; }

        public bool IsChecked
        {
            get => _isChecked;
            set {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
    }
}
