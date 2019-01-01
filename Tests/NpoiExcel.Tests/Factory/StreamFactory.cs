using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;

namespace NpoiExcel.Tests.Factory
{
    public static class StreamFactory
    {
        public static Stream GetExcelMemoryStream()
        {
            var book = new XSSFWorkbook();

            var row = book.CreateSheet ("0").CreateRow (0);

            row.CreateCell(0).SetCellValue("Some data");
            row.CreateCell(1).SetCellValue("Some data");

            var stream = new MemoryStream();
            book.Write( stream, true );
            stream.Position = 0;

            return stream;
        }
    }
}
