using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Business.Interfaces;
using WorkSpeed.Business.ProductivityIndicatorsModels;

namespace WorkSpeed.Business
{
    public class BreakConstraintsForNotSmokers : IBreakConstraints
    {
        public ProductivityTime TryModify ( ProductivityTime timer )
        {
            throw new NotImplementedException();
        }
    }
}
