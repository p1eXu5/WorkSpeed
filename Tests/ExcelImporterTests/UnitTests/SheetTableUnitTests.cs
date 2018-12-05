using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImporter;
using ExcelImporterTests.Factory;
using Moq;
using NPOI.SS.UserModel;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
        public (CellPoint, CellPoint) Ctor_EmptySheet_ReturnsExpectedStartAndEndCells(ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);
            return (sheetTable.StartCell, sheetTable.EndCell);
        }

        #region Factory

        private readonly Mock<ISheet> _mockISheet = new Mock<ISheet>();

        private SheetTable GetSheetTable()
        {
            return new SheetTable(_mockISheet.Object);
        }

        #endregion
    }
}
