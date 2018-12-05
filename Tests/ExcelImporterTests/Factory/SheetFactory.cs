using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImporter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;

namespace ExcelImporterTests.Factory
{
    public class SheetFactory
    {
        public static ISheet EmptySheet => new XSSFSheet();

        public static IEnumerable TestCases(int column, int row)
        {
            var startCell = new[] { column, row };
            var area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 1, 0, 0, 0 },
                };

            yield return new TestCaseData (EmptySheet).Returns ( (new CellPoint(column + 0, row + 0), new CellPoint(column + 3, row + 3)) );

            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 },
                };

            yield return new TestCaseData (EmptySheet).Returns ( (new CellPoint(column + 0, row + 0), new CellPoint(column + 3, row + 3)) );

            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 1, 1, 0, 0 },
                    { 1, 0, 1, 1 },
                    { 1, 0, 0, 0 },
                };

            area = new[,]
                {
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                };

            area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 1, 0, 1 },
                    { 1, 0, 1, 1 },
                    { 0, 0, 0, 1 },
                };

            area = new[,]
                {
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                };

            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 0, 0 },
                };

            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };

            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
        }
    }
}
