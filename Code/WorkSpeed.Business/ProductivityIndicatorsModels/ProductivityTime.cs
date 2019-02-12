using System;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
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
