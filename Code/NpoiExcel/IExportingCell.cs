using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NpoiExcel
{
    public interface IExportingCell
    {
        int Row { get; }
        int Column { get; }

        XSSFColor Color { get; }
        CellType CellType { get; }

        bool IsColorized { get; }

        string GetStringValue ();
        double GetDoubleValue ();
        bool GetBooleanValue ();
        DateTime GetDateTimeValue ();

        object GetValue ();
    }
}
