using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class ShipmentAction : EmployeeAction
    {
        public float Weight { get; set; }
        public float Volume { get; set; }
        public float ClientCargoQuantity { get; set; }
        public float CommonCargoQuantity { get; set; }
    }
}
