using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiExcel
{
    public interface ITypeConverter< in TIn, out TOut >
    {
        TOut Convert ( TIn obj );
    }
}
