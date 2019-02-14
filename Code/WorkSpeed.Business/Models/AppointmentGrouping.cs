using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models
{
    public class AppointmentGrouping
    {
        public AppointmentGrouping ( Appointment appointment, (Position position, Employee[] employees)[] positions )
        {
            Appointment = appointment;
            PositionGrouping = positions.Select( p => new PositionGrouping( p.position, p.employees ) ).ToArray();
        }

        public Appointment Appointment { get; }
        public PositionGrouping[] PositionGrouping { get; }
    }
}
