using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public struct ProductivityTimer
    {
        public TimeSpan Duration;
        public DateTime EndTime;

        public static ProductivityTimer operator + ( ProductivityTimer pt1, ProductivityTimer pt2 )
        {
            pt1.Duration += pt2.Duration;
            pt1.EndTime = pt2.EndTime;

            return pt1;
        }
    }
}
