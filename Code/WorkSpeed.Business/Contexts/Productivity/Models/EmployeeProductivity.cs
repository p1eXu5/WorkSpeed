using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity.Models
{
    public class EmployeeProductivity : IEmployeeProductivity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="productivities">Tuple with productivities and downtimes.</param>
        public EmployeeProductivity ( Employee employee, (IReadOnlyDictionary< Operation, IProductivity >, HashSet< Period > ) productivities )
        {
            Employee = employee;
            (Productivities, DowntimePeriods) = productivities;
        }

        public Employee Employee { get; set; }

        public IProductivity this[ Operation operation ]
        {
            get {
                return Productivities.ContainsKey( operation )
                           ? Productivities[ operation ]
                           : new Productivity();
            }
        }

        public IReadOnlyDictionary< Operation, IProductivity > Productivities { get; }

        public HashSet< Period > DowntimePeriods { get; }


        public IEnumerable< (double count, Operation operation) > GetTimes ( IEnumerable< Operation > operations )
        {
            throw new NotImplementedException();
        }


        public double GetTotalHours ()
        {
            throw new NotImplementedException();
        }
    }
}
