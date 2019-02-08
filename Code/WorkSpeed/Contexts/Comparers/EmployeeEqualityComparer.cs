using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public class EntityEqualityComparer< TEntity,TId > : IEqualityComparer< TEntity >
        where TEntity : IKeyedEntity< TId >
    {
        public bool Equals ( TEntity x, TEntity y )
        {
            if ( ReferenceEquals( x, null ) && ReferenceEquals( y, null ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Id.Equals( y.Id );
        }

        public int GetHashCode ( TEntity obj )
        {
            if ( obj.Id is string ) {
                return  obj.Id?.GetHashCode() ?? 0;
            }

            return obj.Id.GetHashCode();
        }
    }
}
