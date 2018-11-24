using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WorkSpeed.Import
{
    public class ExcelImporter : IConcreteImporter
    {
        private static ExcelImporter _excelImporter;

        private ExcelImporter() {}
        public static ExcelImporter ExcelImporterInstance => _excelImporter ?? (_excelImporter = new ExcelImporter());

        public string FileExtension { get; } = ".xlsx";
        public Func<string, Type, ICollection> ImportData { get; } = ImportDataFromExcel;


        public static ICollection ImportDataFromExcel(string fileName, Type type)
        {
            using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                IWorkbook book = new XSSFWorkbook(stream);
                ISheet sheet = book.GetSheetAt (0);

                var firstCell = GetFirstCell(sheet);
                
                if (-1 == firstCell) {

                    Type t = typeof(List<>);
                    var constr = t.MakeGenericType (type);
                    return (ICollection)Activator.CreateInstance (constr);
                }
            }

            return null;
        }

        public static int GetFirstCell(ISheet sheet)
        {
            return -1;
        }
    }
}
