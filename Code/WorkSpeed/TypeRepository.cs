using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed
{
    public class TypeRepository : ITypeRepository
    {
        public Type GetType (IEnumerable<string> names, Type[] includingAttributes, Type[] excludingAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
