using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiExcel
{
    public interface IDataImporter
    {
        SheetTable ImportData ( string fileName, int sheetIndex );

        IEnumerable< TOutType > GetEnumerable< TIn, TOutType > (

            SheetTable                                        sheetTable,
            Dictionary< string, (string header, int column) > propertyMap,
            ITypeConverter< TIn, TOutType >                   typeConverter 
        );

        event EventHandler< ProgressChangedEventArgs > ProgressChangedEvent;
    }
}
