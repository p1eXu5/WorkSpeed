using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public class ShipmentActionComparer : IEqualityComparer< ShipmentAction >
    {
        public bool Equals ( ShipmentAction x, ShipmentAction y )
        {
            if ( ReferenceEquals( x, null ) && ReferenceEquals( y, null ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Id.Equals( y.Id ) && x.EmployeeId.Equals( y.EmployeeId );
        }

        public int GetHashCode ( ShipmentAction obj )
        {
            int hash = 17;
            hash = hash * 23 + obj.Id.GetHashCode();
            hash = hash * 23 + obj.EmployeeId.GetHashCode();
            return hash;
        }
    }
}
