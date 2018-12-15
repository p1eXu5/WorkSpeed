using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class GatheringAction : WithProductAction
    {
        public Address SenderCellAdress { get; set; }
        public Address ReceiverCellAdress { get; set; }
    }
}
