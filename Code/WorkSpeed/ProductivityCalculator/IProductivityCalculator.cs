using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityCalculator
{
    public interface IProductivityCalculator<TAction> where TAction : EmployeeAction
    {
        void Calculate (SortedSet<TAction> actions, ICollection<Productivity2> productivities);
    }
}
