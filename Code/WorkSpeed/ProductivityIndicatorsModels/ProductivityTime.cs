using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public struct ProductivityTime
    {
        public TimeSpan Duration;
        public DateTime EndTime;
        public readonly OperationGroups Operation;

        public ProductivityTime ( OperationGroups operation )
        {
            Operation = operation;
            Duration = default( TimeSpan );
            EndTime = default( DateTime );
        }

        public static ProductivityTime operator + ( ProductivityTime pt1, ProductivityTime pt2 )
        {
            pt1.Duration += pt2.Duration;
            pt1.EndTime = pt2.EndTime;

            return pt1;
        }
    }
}
