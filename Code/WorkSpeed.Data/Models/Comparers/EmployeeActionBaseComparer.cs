
using System.Collections.Generic;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.Comparers
{
    public class EmployeeActionBaseComparer<T> : IComparer<T>, IEqualityComparer< T >
        where T : EmployeeActionBase
    {
        public int Compare (T x, T y)
        {
            if (object.Equals (x, y)) return 0;
            return x.StartTime.CompareTo (y.StartTime);
        }

        public bool Equals ( T x, T y )
        {
            if (x == null || y == null) return ReferenceEquals (x, y);
            return x.StartTime.Equals (y.StartTime);
        }

        public int GetHashCode ( T obj )
        {
            return obj.StartTime.GetHashCode();
        }
    }
}
