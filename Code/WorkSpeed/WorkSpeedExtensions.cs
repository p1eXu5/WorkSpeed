using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed
{
    public static class WorkSpeedExtensions
    {
        public static TimeSpan Sum (this IEnumerable<TimeSpan> times)
        {
            if (times == null) throw new ArgumentNullException();

            var sum = TimeSpan.Zero;

            foreach (var time in times) {
                sum += time;
            }

            return sum;
        }
    }
}
