using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class WithProductAction : EmployeeAction
    {
        public Product Product { get; set; }
        public ushort ProductQuantity { get; set; }

        public float Volume => Product.Volume;
        public float Weight => Product.Weight;
    }
}
