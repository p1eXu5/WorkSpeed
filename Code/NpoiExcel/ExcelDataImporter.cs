using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiExcel
{
    public class ExcelDataImporter : IDataImporter
    {
        public SheetTable ImportData ( string fileName, int sheetIndex )
        {
            return ExcelImporter.ImportData( fileName, sheetIndex );
        }
    }
}
