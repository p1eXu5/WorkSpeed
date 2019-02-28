using System;
using Agbm.Wpf.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering
{
    public class FilterItemViewModel : ViewModel
    {
        private bool _isChecked;

        public FilterItemViewModel ( object entity, Func< object,string > caption )
        {
            Entity = entity;
            Caption = caption.Invoke( entity );
        }

        public object Entity { get; }
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
