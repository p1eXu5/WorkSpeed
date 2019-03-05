using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity.Comparers
{
    public class ProductEqualityComparer : IEqualityComparer< Product >
    {
        public bool Equals ( Product x, Product y )
        {
            if (x == null || y == null) return ReferenceEquals (x, y);
            return x.Id.Equals (y.Id);
        }

        public int GetHashCode ( Product obj )
        {
            throw new NotImplementedException();
        }
    }
}
