using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Comparers
{
    public class EmployeeActionComparer<T> : IComparer<T>
        where T : EmployeeAction
    {
        public int Compare (T x, T y)
        {
            if (object.Equals (x, y)) return 0;
            return x.StartTime.CompareTo (y.StartTime);
        }
    }
}
