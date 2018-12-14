using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.FileModels
{
    public class ActionImportModelComparer<T> : IComparer<T>
        where T : EmployeeActionImportModel
    {
        public int Compare (T x, T y)
        {
            if (object.Equals (x, y)) {
                return 0;
            }

            return x.DateTime.CompareTo (y.DateTime);
        }
    }
}
