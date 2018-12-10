using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Interfaces
{
    public interface ITypeRepository
    {
        Type GetType (IEnumerable<string> names, Type[] includingAttributes, Type[] excludingAttributes);
    }
}
