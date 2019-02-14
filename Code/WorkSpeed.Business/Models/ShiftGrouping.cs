using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models
{
    public class ShiftGrouping
    {
        public ShiftGrouping ( Shift shift, (Appointment appointment, (Position,Employee[])[] positions)[] appointments )
        {
            Shift = shift;
            Appointments = appointments.Select( a => new AppointmentGrouping( a.appointment, a.positions) ).ToArray();
        }

        public Shift Shift { get; }
        public AppointmentGrouping[] Appointments { get; }
    }
}
