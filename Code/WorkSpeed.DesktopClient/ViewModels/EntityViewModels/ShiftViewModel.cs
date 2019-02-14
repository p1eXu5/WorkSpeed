using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ShiftViewModel : ViewModel
    {
        private readonly ObservableCollection< AppointmentViewModel > _appointments;

        public ShiftViewModel ( ShiftGrouping shift )
        {
            Shift = shift.Shift;
            _appointments = new ObservableCollection< AppointmentViewModel >( shift.Appointments.Select( a => new AppointmentViewModel( a ) ) );
            Appointments = new ReadOnlyObservableCollection< AppointmentViewModel >( _appointments );
        }

        public Shift Shift { get; }
        public ReadOnlyObservableCollection< AppointmentViewModel > Appointments { get; }
    }
}
