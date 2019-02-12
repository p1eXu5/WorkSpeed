using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
{
    public class GatheringIndicators
    {
        public int ProductQuantity { get; private set; }
        public int LineQuantity { get; private set; }
        public double Volume { get; private set; }
        public double Weight { get; private set; }

    }
}
