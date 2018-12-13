using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class GatheringAction : WithProductAction
    {
        public Address SenderAdress { get; set; }
        public Address ReceiverAdress { get; set; }

        public ushort ProductQuantity { get; set; }

        public float Volume => Product.Volume;
        public float Weight => Product.Weight;
    }
}
