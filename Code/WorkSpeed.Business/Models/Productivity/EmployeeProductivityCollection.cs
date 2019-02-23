using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public class EmployeeProductivityCollection
    {

        public EmployeeProductivityCollection ( Employee employee, (IReadOnlyDictionary< Operation, IProductivity >, HashSet< Period > ) productivities )
        {
            Employee = employee;
        }

        public Employee Employee { get; set; }

        public IProductivity this[ Operation operation ] => Productivities[ operation ];

        public IReadOnlyDictionary< Operation, IProductivity > Productivities { get; set; }

    }
}
