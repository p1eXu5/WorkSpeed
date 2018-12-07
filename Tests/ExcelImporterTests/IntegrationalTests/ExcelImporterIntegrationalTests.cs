using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImporter.Tests.Factory;
using NUnit.Framework;

namespace ExcelImporter.Tests.IntegrationalTests
{
    [TestFixture]
    public class ExcelImporterIntegrationalTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-us");
        }

        [TestCaseSource(typeof(TypeExcelFactory), nameof(TypeExcelFactory.TestCases))]
        public void ImportData__ExistingFile_CorrespondingType__ReturnsExpectedData (string fileName, Type type, string[,] results)
        {

        }
    }
}
