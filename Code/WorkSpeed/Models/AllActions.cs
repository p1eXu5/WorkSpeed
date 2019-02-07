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
        public ICollection< DoubleAddressAction > DoubleAddressActions { get; set; }
        public ICollection< ReceptionAction > ReceptionActions { get; set; }
        public ICollection< InventoryAction > InventoryActions { get; set; }
        public ICollection< ShipmentAction > ShipmentActions { get; set; }
    }
}
