using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Models.Comparers
{
    public class EntityComparer< TEntity,TId > : IEqualityComparer< TEntity >   where TEntity : IKeyedEntity< TId >
    {
        public bool Equals(TEntity x, TEntity y)
        {
            if (x == null || y == null) return ReferenceEquals(x, y);
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(TEntity obj)
        {
            throw new NotImplementedException();
        }
    }

}
