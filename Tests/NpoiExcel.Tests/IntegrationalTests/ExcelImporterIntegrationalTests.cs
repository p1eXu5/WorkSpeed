using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Tests.Factory;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;

namespace NpoiExcel.Tests.IntegrationalTests
{
    [TestFixture]
    public class ExcelImporterIntegrationalTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-us");
        }

        [Test]
        public void ImportData_ByDefault_ReturnsNotNullSheetTable()
        {
            // Arrange:
            var stream = GetExcelMemoryStream();

            // Action:
            var sheetTable = ExcelImporter.ImportData (stream, 0);

            // Assert:
            Assert.That (sheetTable, Is.Not.Null);
        }


        #region Factory

        private Stream GetExcelMemoryStream()
        {
            var book = new XSSFWorkbook();

            var row = book.CreateSheet ("0").CreateRow (0);

            row.CreateCell(0).SetCellValue("Some data");
            row.CreateCell(1).SetCellValue("Some data");

            var stream = new MemoryStream();
            book.Write (stream, true);
            stream.Position = 0;

            return stream;
        }

        #endregion
    }
}
