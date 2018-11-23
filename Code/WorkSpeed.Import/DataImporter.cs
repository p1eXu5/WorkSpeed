using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public class DataImporter : IDataImporter
    {
        private readonly Dictionary<string,Func<string,object>> _strategies = new Dictionary<string, Func<string,object>>();

        public DataImporter()
        {
            _strategies[".xlsx"] = ImportDataFromExcel;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public ICollection<T> ImportData<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName)) { throw new FileNotFoundException(); }
            
            if (!_strategies.ContainsKey(Path.GetExtension(fileName))) {
                throw new ArgumentException("The source does not handled");
            }

            var typeProperties = typeof(T).GetProperties();
            if (typeProperties.Length == 0) { throw new TypeAccessException(@"Passed type does not have public properties"); }

            return (ICollection<T>)_strategies[Path.GetExtension(fileName)].Invoke(fileName);
        }

        private object ImportDataFromExcel(string fileName)
        {
            return null;
        }
    }
}
