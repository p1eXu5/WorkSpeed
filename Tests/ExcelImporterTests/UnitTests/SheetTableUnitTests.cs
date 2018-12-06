using System;
using System.Globalization;
using System.Linq;
using ExcelImporter;
using ExcelImporterTests.Factory;
using NPOI.SS.UserModel;
using NUnit.Framework;

namespace ExcelImporterTests.UnitTests
{
    [TestFixture]
    public class SheetTableUnitTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-us");
        }

        [Test]
        public void Ctor_SheetIsNull_Throws()
        {
            var ex = Assert.Catch<ArgumentNullException> (() => new SheetTable (null));

            StringAssert.Contains ("Sheet can't be null", ex.Message);
        }

        [Test]
        public void Ctor_EmptySheet_Throws()
        {
            var ex = Assert.Catch<ArgumentException> (() => new SheetTable (SheetFactory.EmptySheet));

            StringAssert.Contains ("Sheet has no data", ex.Message);
        }

        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.TestCases), new object[] { 5, 5})]
        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.TestCases), new object[] { 0, 5})]
        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.TestCases), new object[] { 5, 0})]
        public (CellPoint, CellPoint) Ctor_EmptySheet_ReturnsExpectedStartAndEndCells (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);
            return (sheetTable.StartCell, sheetTable.EndCell);
        }

        [TestCaseSource (typeof (SheetFactory), nameof(SheetFactory.HeaderTestCases), new object[] {5, 5})]
        public void NormalizedHeadersGetter_NotEmptySheet_ReturnsNotEmptyCollection (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            Assert.That (sheetTable.NormalizedHeaders.Any());
        }

        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.HeaderTestCases), new object[] { 5, 5 })]
        public void Indexator__NotEmptySheet_IndexGreaterThanMaxColumn__Throws (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            string header;
            var ex = Assert.Catch<IndexOutOfRangeException> (() => header = sheetTable[sheetTable.EndCell.Column]);

            StringAssert.Contains("Column was outside the bounds of sheet table!", ex.Message);
        }

        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.HeaderTestCases), new object[] { 5, 5 })]
        public void Indexator__NotEmptySheet_IndexLessThanMinColumn__Throws (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            // ReSharper disable once NotAccessedVariable
            string header;
            var ex = Assert.Catch<IndexOutOfRangeException>(() => header = sheetTable[sheetTable.StartCell.Column - 1]);

            StringAssert.Contains("Column was outside the bounds of sheet table!", ex.Message);
        }

        [TestCaseSource(typeof(SheetFactory), nameof(SheetFactory.HeaderTestCasesWithReturns), new object[] { 5, 5 })]
        public string[] Indexator__NotEmptySheet_ValidIndex__ReturnsColumnHeader (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            string[] headers = new string[sheetTable.EndCell.Column - sheetTable.StartCell.Column];

            for (int i = sheetTable.StartCell.Column; i < sheetTable.EndCell.Column; i++) {

                headers[i - sheetTable.StartCell.Column] = sheetTable[i];
            }

            return headers;
        }
    }
}
