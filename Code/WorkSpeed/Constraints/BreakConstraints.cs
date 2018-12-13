using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.Constraints
{
    public abstract class BreakConstraints
    {
        public static IBreakConstraints DefaultBreakConstraints => new DefaultBreakConstraints();
    }

    internal sealed class DefaultBreakConstraints : IBreakConstraints
    {
        public ProductivityTime TryModify ( ProductivityTime timer ) => timer;
    }
}
