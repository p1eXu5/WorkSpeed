using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class GatheringAction : EmployeeAction
    {
        public Product Product { get; set; }

        public Address SenderAdress { get; set; }
        public Address ReceiverAdress { get; set; }

        public ushort ProductQuantity { get; set; }

    }
}
