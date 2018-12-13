using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.Interfaces
{
    public interface IBreakConstraints
    {
        ProductivityTimer TryModify ( ProductivityTimer timer );
    }
}
