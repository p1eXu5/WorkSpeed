using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class InventoryActionDetail : SingleAddressDetail
    {
         public int AccountingQuantity { get; set; }

        public int InventoryActionId { get; set; }
        public InventoryAction InventoryAction { get; set; }
    }
}
