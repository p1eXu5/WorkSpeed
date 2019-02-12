using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.ProductivityIndicatorsModels;

namespace WorkSpeed.Business.Constraints
{
    public interface IBreakConstraints
    {
        ProductivityTime TryModify ( ProductivityTime timer );
    }
}
