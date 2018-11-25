using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using WorkSpeed.Import.Attributes;
using static WorkSpeed.Import.Tests.TestHelper;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    public class ExcelImporterUnitTests
    {
        #region GetFirstCell

        [Test]
        public void ImportDataFromExcel_XlsxFileDoesNotContainData_ReturnsEmptyCollection()
        {
            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(GetFullPath ("empty.xlsx"), typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_ZeroLenghtFileWithCorrectExtension_ReturnsEmptyCollection()
        {
            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(CreateFakeZeroLengthXlsxFile (), typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_OneColumnOneProperty_ReturnsTheMostTopLeftCell ()
        {
            // Arrange:
            var fileName = CreateTestFile ();

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeModelClass));

            // Assert:
            Assert.That (_mainCellText == resColl.OfType<FakeModelClass>().First().MainCell);
        }

        [Test]
        public void ImportDataFromExcel_MultiColumnOneProperty_ReturnsEmptyCollection ()
        {
            // Arrange:
            var fileName = CreateTestFile (new TableRect(5, 5, 5), (float)1.0, 5, 5);

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }


        [TearDown]
        public void Cleanup()
        {
            RemoveFakeFile();
        }

        // TODO: Property Count Assertions

        #endregion

        #region Factory

        private const string _fakeFileName = "fakefile.xlsx";
        private const string _mainCellText = "Main Cell";

        string CreateFakeZeroLengthXlsxFile()
        {
            using (var stream = new FileStream (GetFullPath(_fakeFileName),FileMode.Create, FileAccess.Write)) {
                return stream.Name;
            }
        }

        void RemoveFakeFile()
        {
            if (File.Exists (GetFullPath(_fakeFileName))) {
                File.Delete (_fakeFileName);
            }
        }

        private string CreateTestFile (TableRect columns, float density, byte mainTop, byte mainLeft)
        {
            Random rnd = new Random((int)DateTime.Now.TimeOfDay.TotalSeconds);
            try {
                IWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet();

                var Nc = columns.Right - columns.Left;
                var nc = density > 1 
                            ? (density - Math.Floor (density)) * Nc 
                            : density * Nc;

                for (int j = columns.Top; j <= columns.Bottom; j++) {

                    var row = sheet.CreateRow (j);

                    for (int i = 0; i < nc; i++) {

                        int nextCell = columns.Left + rnd.Next (Nc);

                        while (!(null == row.GetCell (nextCell))) {
                            ++nextCell;
                            if (nextCell == columns.Right) {
                                nextCell = columns.Left;
                            }
                        }

                        row.CreateCell (nextCell).SetCellValue ("value");
                    }
                }

                if (sheet.GetRow (mainTop)?.GetCell (mainLeft) == null) {
                    sheet.CreateRow (mainTop).CreateCell (mainLeft).SetCellValue (_mainCellText);
                }
                else {
                    sheet.GetRow (mainTop).GetCell (mainLeft).SetCellValue (_mainCellText);
                }

                using (var stream = new FileStream (GetFullPath (_fakeFileName), FileMode.Create, FileAccess.Write)) {
                    
                    book.Write (stream);
                    return stream.Name;
                }
            }
            catch {
                throw;
            }
        }

        private string CreateTestFile()
        {
            try {
                IWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet();

                sheet.CreateRow (0).CreateCell (0).SetCellValue ("");
                sheet.CreateRow (10).CreateCell (10).SetCellValue (_mainCellText);

                using (var stream = new FileStream (GetFullPath (_fakeFileName), FileMode.Create, FileAccess.Write)) {
                    
                    book.Write (stream);
                    return stream.Name;
                }
            }
            catch {
                throw;
            }
        }

        [Headless]
        class FakeModelClass
        {
            public string MainCell { get; set; }
        }

        #endregion
    }
}
