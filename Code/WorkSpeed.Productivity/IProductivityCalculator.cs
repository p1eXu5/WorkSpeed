using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;

namespace WorkSpeed.Productivity
{
    public interface IProductivityCalculator
    {
        IEnumerable< Productivity > CalculateProductivity ();
    }
}
