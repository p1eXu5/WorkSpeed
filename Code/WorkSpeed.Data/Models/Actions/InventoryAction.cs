using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class InventoryAction : WithProductAction
    {
        public int AccountingQuantity { get; set; }
        public Address InventoryCellAddress { get; set; }
    }
}
