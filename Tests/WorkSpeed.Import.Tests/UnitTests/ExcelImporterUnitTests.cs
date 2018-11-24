using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static WorkSpeed.Import.Tests.TestHelper;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    public class ExcelImporterUnitTests
    {
        [Test]
        public void GetFirstCell_XlsxFileDoesntContainData_ReturnsEmptyCollection()
        {
            // Arrange:
            var importer = GetExcelImporter();

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(GetFullPath ("empty.xlsx"), typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }


        #region Factory

        ExcelImporter GetExcelImporter()
        {
            return ExcelImporter.ExcelImporterInstance;
        }

        class FakeModelClass
        {

        }

        #endregion
    }
}
