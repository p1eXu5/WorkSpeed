using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public class AddressEqualityComparer : IEqualityComparer< Address >
    {
        public bool Equals ( Address x, Address y )
        {
            if ( ReferenceEquals( x, null ) && ReferenceEquals( y, null ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Letter.Equals( y.Letter ) && x.Row == y.Row && x.Section == y.Section && x.Shelf == y.Shelf && x.Box == y.Box;
        }

        public int GetHashCode ( Address obj )
        {
            int hash = 17;
            hash = hash * 23 + obj.Letter.GetHashCode();
            hash = hash * 23 + obj.Row.GetHashCode();
            hash = hash * 23 + obj.Section.GetHashCode();
            hash = hash * 23 + obj.Shelf.GetHashCode();
            hash = hash * 23 + obj.Box.GetHashCode();
            return hash;
        }
    }
}
