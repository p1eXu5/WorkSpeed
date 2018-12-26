using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Productivity
{
    public interface IBreakRepository
    {
        string[] GetNames ();
        string Check ( Period period );
    }
}
