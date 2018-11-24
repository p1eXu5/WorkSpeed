using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public interface IDataImporter
    {
        IEnumerable<T> ImportData<T>(string fileName)  where T : new();
        IEnumerable<T> ImportDataAsync<T> (string fileName) where T : new ();
    }
}
