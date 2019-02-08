using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public class EmployeeEqualityComparer : IEqualityComparer< Employee >
    {
        public bool Equals ( Employee x, Employee y )
        {
            if ( ReferenceEquals( x, null ) && ReferenceEquals( y, null ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Id.Equals( y.Id );
        }

        public int GetHashCode ( Employee obj )
        {
            return obj.Id?.GetHashCode() ?? 0;
        }
    }
}
