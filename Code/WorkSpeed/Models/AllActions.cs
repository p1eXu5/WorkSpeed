using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Models
{
    public class AllActions : IEntity
    {
        public DoubleAddressAction DoubleAddressAction { get; set; }
        public ReceptionAction ReceptionAction { get; set; }
        public InventoryAction InventoryAction { get; set; }
        public ShipmentAction ShipmentAction { get; set; }
        public OtherAction OtherAction { get; set; }
    }
}
