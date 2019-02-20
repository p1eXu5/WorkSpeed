
using System.Collections.Generic;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.Comparers
{
    public class EmployeeActionComparer<T> : IComparer<T>
        where T : EmployeeActionBase
    {
        public int Compare (T x, T y)
        {
            if (object.Equals (x, y)) return 0;
            return x.StartTime.CompareTo (y.StartTime);
        }
    }
}
