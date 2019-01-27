using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class ShipmentActionDetails : TimeActionDetails
    {
        //public override void AddDetails ( EmployeeActionBase action, TimeSpan pause )
        //{
        //    base.AddDetails( action, pause );

        //    if ( !(action is ShipmentAction shipment) ) return;

        //    //Weight += shipment.Weight;
        //    //Volume += shipment.Volume;
        //}

        public double Weight { get; private set; }
        public double Volume { get; private set; }
    }
}
