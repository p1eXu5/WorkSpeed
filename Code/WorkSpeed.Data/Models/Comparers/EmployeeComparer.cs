using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Models.Comparers
{
    public class EmployeeComparer : IEqualityComparer<Employee>
    {
        public bool Equals (Employee x, Employee y)
        {
            if (x == null || y == null) return ReferenceEquals (x, y);
            return x.Id.Equals (y.Id);
        }

        public int GetHashCode (Employee obj)
        {
            throw new NotImplementedException();
        }
    }
}
