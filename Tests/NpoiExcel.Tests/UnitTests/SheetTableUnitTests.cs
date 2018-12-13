﻿using System;
using System.Globalization;
using System.Linq;
using NpoiExcel;
using NpoiExcel.Tests.Factory;
using NPOI.SS.UserModel;
using NUnit.Framework;

namespace NpoiExcel.Tests.UnitTests
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

            StringAssert.Contains ("sheet can't be null", ex.Message);
        }

        [Test]
        public void Ctor_EmptySheet_Throws()
        {
            var ex = Assert.Catch<ArgumentException> (() => new SheetTable (MockedSheetFactory.EmptySheet));

            StringAssert.Contains ("sheet has no data", ex.Message);
        }

        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.RowColumnCountsTestCases), new object[] { 5, 5})]
        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.RowColumnCountsTestCases), new object[] { 0, 5})]
        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.RowColumnCountsTestCases), new object[] { 5, 0})]
        public (int, int) Ctor_EmptySheet_ReturnsExpectedRowAndColumnLenghts (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);
            return (sheetTable.RowCount, sheetTable.ColumnCount);
        }

        [TestCaseSource (typeof (MockedSheetFactory), nameof(MockedSheetFactory.HeaderTestCases), new object[] {5, 5})]
        public void NormalizedHeadersGetter_NotEmptySheet_ReturnsNotEmptyCollection (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            Assert.That (sheetTable.NormalizedHeaders.Any());
        }

        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.HeaderTestCases), new object[] { 5, 5 })]
        public void GetNormalizedHeaderAt__NotEmptySheet_IndexGreaterThanMaxColumn__Throws (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            string header;
            var ex = Assert.Catch<IndexOutOfRangeException> (() => header = sheetTable.GetNormalizedHeaderAt (sheetTable.ColumnCount));

            StringAssert.Contains("Column was outside the bounds of sheet table!", ex.Message);
        }

        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.HeaderTestCases), new object[] { 5, 5 })]
        public void GetNormalizedHeaderAt__NotEmptySheet_IndexLessThanMinColumn__Throws (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            // ReSharper disable once NotAccessedVariable
            string header;
            var ex = Assert.Catch<IndexOutOfRangeException>(() => header = sheetTable.GetNormalizedHeaderAt (-1));

            StringAssert.Contains("Column was outside the bounds of sheet table!", ex.Message);
        }

        [TestCaseSource(typeof(MockedSheetFactory), nameof(MockedSheetFactory.HeaderTestCasesWithReturns), new object[] { 5, 5 })]
        public string[] GetNormalizedHeaderAt__NotEmptySheet_ValidIndex__ReturnsColumnHeader (ISheet sheet)
        {
            var sheetTable = new SheetTable(sheet);

            string[] headers = new string[sheetTable.ColumnCount];

            for (int i = 0; i < sheetTable.ColumnCount; i++) {

                headers[i] = sheetTable.GetNormalizedHeaderAt (i);
            }

            return headers;
        }

        [Test]
        public void Indexer_RowOutOfRange_Throws()
        {
            // Arrange:
            var sheet = MockedSheetFactory.HeaderTestCases (5, 5).Cast<TestCaseData>().First().Arguments[0] as ISheet;
            var sheetTable = new SheetTable(sheet);

            // Action:
            var ex = Assert.Catch<IndexOutOfRangeException>(() =>
                {
                    var res = sheetTable[-1, 0];
                });

            // Assert:
            StringAssert.Contains ("Row was outside the bounds of sheet table.", ex.Message);
        }

        [Test]
        public void Indexer_ColumnOutOfRange_Throws()
        {
            // Arrange:
            var sheet = MockedSheetFactory.HeaderTestCases (5, 5).Cast<TestCaseData>().First().Arguments[0] as ISheet;
            var sheetTable = new SheetTable(sheet);

            // Action:
            var ex = Assert.Catch<IndexOutOfRangeException>(() =>
                {
                    var res = sheetTable[0, -1];
                });

            // Assert:
            StringAssert.Contains ("Column was outside the bounds of sheet table.", ex.Message);
        }

        [Test]
        public void Indexer_SheetTableContainsOnlyHeaders_Throws()
        {
            // Arrange:
            var sheet = MockedSheetFactory.HeaderTestCases (5, 5).Cast<TestCaseData>().Where (tc => tc.TestName.Contains ("#8"))?.First().Arguments[0] as ISheet;
            var sheetTable = new SheetTable(sheet);

            // Action:
            var ex = Assert.Catch<InvalidOperationException>(() =>
                {
                    var res = sheetTable[0, 0];
                });

            // Assert:
            StringAssert.Contains ("Sheet has only headers.", ex.Message);
        }

        [Test]
        public void Indexer_ValidRowAndColumn_ReturnsCellValue()
        {
            // Arrange:
            var sheet = MockedSheetFactory.HeaderTestCases (5, 5).Cast<TestCaseData>().First().Arguments[0] as ISheet;
            var sheetTable = new SheetTable(sheet);

            // Action:
            var res = sheetTable[2, 2];

            // Assert:
            Assert.That (res, Is.Not.Null);
            Assert.That (res.GetType().IsAssignableFrom (typeof(CellValue)));
        }
    }
}