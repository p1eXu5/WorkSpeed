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
    }
}
