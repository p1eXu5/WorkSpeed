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

        public IProductivity this[ Operation operation ] => Productivities.ContainsKey( operation )
                                                                ? Productivities[ operation ]
                                                                : new Productivity();

        public IReadOnlyDictionary< Operation, IProductivity > Productivities { get; }

        public HashSet< Period > DowntimePeriods { get; }


        public double GetTotalDowntimeHours ()
        {
            return DowntimePeriods.Where( p => p.Duration < TimeSpan.FromHours( 5 ) ).Sum( dt => dt.Duration.TotalHours );
        }

        public IEnumerable< (double hours, Operation operation) > GetOperationTimes ( IEnumerable< Operation > operations )
        {
            var opArr = operations.ToArray();
            var res = new (double hours, Operation operation)[ opArr.Length ];

            for ( var i = 0; i < opArr.Length; ++i ) {

                if ( Productivities.ContainsKey( opArr[ i ] ) ) {
                    res[i] = ( Productivities[ opArr[i] ].GetTotalHours(), opArr[i] );
                }
                else {
                    res[i] = ( 0.0, opArr[i] );
                }
            }

            return res;
        }


        public double GetTotalWorkHours ()
        {
            return Productivities.Values.Sum( v => v.GetTotalHours() );
        }
    }
}
