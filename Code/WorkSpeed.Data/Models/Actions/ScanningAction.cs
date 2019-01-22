using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class ScanningAction : WithProductAction
    {
        public ushort ScanQuantity { get; set; }
        public Address ReceptionDynamicCellAdress { get; set; }
    }
}
