using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public interface IConcreteImporter
    {
        HashSet<string> FileExtensions { get; }
        Func<string,Type,ICollection> ImportDataFunc { get; }
    }
}
