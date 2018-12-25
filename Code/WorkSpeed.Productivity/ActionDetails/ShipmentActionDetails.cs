using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class ShipmentActionDetails : TimeActionDetails
    {
        public override void AddDetails ( EmployeeAction action, TimeSpan pause )
        {
            base.AddDetails( action, pause );

            if ( !(action is ShipmentAction shipment) ) return;

            Weight += shipment.Weight;
            Volume += shipment.Volume;
        }

        public double Weight { get; private set; }
        public double Volume { get; private set; }
    }
}
