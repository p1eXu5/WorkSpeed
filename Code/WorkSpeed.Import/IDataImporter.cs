using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public interface IDataImporter
    {
        ICollection<T> ImportData<T>(string fileName)  where T : new();
    }
}
