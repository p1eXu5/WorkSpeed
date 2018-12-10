using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityCalculator
{
    public class ProductivityCalculator : IProductivityCalculator
    {
        public Productivity CalculatePoductivities (IEnumerable<EmployeeAction> actions)
        {
            throw new NotImplementedException();
        }
    }
}
