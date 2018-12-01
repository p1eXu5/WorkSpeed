using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public class TypeRepository : ITypeRepository
    {
        public static Type GetTypeByHeaderNames(IEnumerable<string> headers)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Type> GetTypeByColumnCount(int count)
        {
            throw new NotImplementedException();
        }
    }
}
