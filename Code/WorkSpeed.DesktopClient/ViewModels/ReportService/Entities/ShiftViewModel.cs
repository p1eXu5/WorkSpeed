using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class ShiftViewModel : ViewModel
    {
        private readonly Shift _shift;

        public ShiftViewModel ( Shift shift )
        {
            _shift = shift;
        }

        public Shift Shift => _shift;

        public string Name
        {
            get => _shift.Name;
            set {
                _shift.Name = value;
                OnPropertyChanged();
            }
        }
    }
}
