using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class ReceptionAction : WithProductAction
    {
        public int  ScanQuantity { get; set; }
        public bool IsClientScanning { get; set; }

        public Address ReceiverCellAddress { get; set; }
    }
}
