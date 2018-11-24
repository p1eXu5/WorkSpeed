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
        string FileExtension { get; }
        Func<string,Type,ICollection> ImportData { get; }
    }
}
