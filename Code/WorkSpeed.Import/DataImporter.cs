using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WorkSpeed.Import
{
    public sealed class DataImporter : IDataImporter
    {
        private readonly Dictionary<string,Func<string,Type,object>> _strategies = new Dictionary<string, Func<string,Type,object>>();

        public DataImporter()
        {
            _strategies[".xlsx"] = ImportDataFromExcel;
        }

        public IEnumerable<T> ImportDataAsync<T> (string fileName) where T : new()
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<T> ImportData<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName)) { throw new FileNotFoundException(); }
            
            if (!_strategies.ContainsKey(Path.GetExtension(fileName))) {
                throw new ArgumentException("The source does not handled");
            }

            var typeProperties = typeof(T).GetProperties();
            if (0 == typeProperties.Length) { throw new TypeAccessException(@"Passed type does not have public properties"); }

            return (IEnumerable<T>)_strategies[Path.GetExtension(fileName)].Invoke(fileName, typeof(T));
        }

        private object ImportDataFromExcel(string fileName, Type type)
        {
            using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                IWorkbook book = new XSSFWorkbook(stream);
                ISheet sheet = book.GetSheetAt (0);

                var firstCell = GetFirstCell(sheet);
                
                if (-1 == firstCell) {

                    Type t = typeof(List<>);
                    var constr = t.MakeGenericType (type);
                    return Activator.CreateInstance (constr);
                }
            }

            return null;
        }

        private int GetFirstCell(ISheet sheet)
        {
            return -1;
        }
    }
}
