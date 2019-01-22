using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class ReceptionActionDetail : SingleAddressDetail
    {
        public ushort ScanQuantity { get; set; }
        public bool IsClientScanning { get; set; }
    }
}
