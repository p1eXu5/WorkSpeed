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
    public class AppointmentViewModel : ViewModel
    {
        private readonly ObservableCollection< PositionViewModel > _positions;

        public AppointmentViewModel ( AppointmentGrouping appointment )
        {
            Appointment = appointment.Appointment;
            _positions = new ObservableCollection< PositionViewModel >( appointment.PositionGrouping.Select( p => new PositionViewModel( p ) ) );
            Positions = new ReadOnlyObservableCollection< PositionViewModel >( _positions );
        }

        public Appointment Appointment { get;}
        public ReadOnlyObservableCollection< PositionViewModel > Positions { get; }
    }
}
