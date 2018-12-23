using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiExcel
{
    public class ExcelDataImporter : IDataImporter
    {
        public event EventHandler< ProgressChangedEventArgs > ProgressChangedEvent
        {
            add => ExcelImporter.ProgressChangedEvent += value;
            remove => ExcelImporter.ProgressChangedEvent -= value;
        }

        public SheetTable ImportData ( string fileName, int sheetIndex )
        {
            return ExcelImporter.ImportData( fileName, sheetIndex );
        }

        public IEnumerable< TOutType > GetEnumerable< TIn, TOutType > ( SheetTable sheetTable,  
                                                                        Dictionary< string, (string header, int column) > propertyMap,  
                                                                        ITypeConverter< TIn, TOutType > typeConverter )
        {
            return ExcelImporter.GetEnumerable( sheetTable, propertyMap, typeConverter );
        }
    }
}
