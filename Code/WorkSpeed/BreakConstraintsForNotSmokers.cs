using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed
{
    public class BreakConstraintsForNotSmokers : IBreakConstraints
    {
        public ProductivityTime TryModify ( ProductivityTime timer )
        {
            throw new NotImplementedException();
        }
    }
}
