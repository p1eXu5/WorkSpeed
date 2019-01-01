using System;
using System.Globalization;
using System.IO;
using NpoiExcel.Tests.Factory;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NpoiExcel;
using NUnit.Framework;
using static Helpers.StringExtensions;

namespace NpoiExcel.Tests.UnitTests
{
    [TestFixture]
    public class ExcelImporterUnitTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-Us", false);
        }

        #region ImportData With FileName

        [Test]
        public void ImportData_FileNameIsNull_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            string path = null;
            var ex = Assert.Catch<ArgumentException> (() => ExcelImporter.ImportData(path, type, 0));

            StringAssert.Contains ("Path cannot be null.", ex.Message);
        }

        [Test]
        public void ImportData_FileNameIsEmptyString_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var ex = Assert.Catch<ArgumentException>(() => ExcelImporter.ImportData(String.Empty, type, 0));

            StringAssert.Contains("Empty path name is not legal.", ex.Message);
        }

        [Test]
        public void ImportData_FileNameIsWhitespaces_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var ex = Assert.Catch<ArgumentException>(() => ExcelImporter.ImportData("  ", type, 0));

            StringAssert.Contains("The path is not of a legal form.", ex.Message);
        }

        [Test]
        public void ImportData_FileNameConsistsOfExtensionOnly_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var ex = Assert.Catch<FileNotFoundException>(() => ExcelImporter.ImportData(".xlsx", type, 0));

            StringAssert.Contains("Could not find file", ex.Message);
        }

        [Test]
        public void ImportData_FileDoesntExist_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var ex = Assert.Catch<FileNotFoundException>(() => ExcelImporter.ImportData("notexistedfile.xlsx", type, 0));

            StringAssert.Contains("Could not find file", ex.Message);
        }

        [Test]
        public void ImportData_FileDoesNotContainExcelData_Throws()
        {
            var file = "test.txt".AppendAssemblyPath();

            var stream = File.Create (file);
            stream.Close();

            var type = TypeExcelFactory.EmptyClass;
            var ex = Assert.Catch<FileFormatException>(() => ExcelImporter.ImportData(file, type, 0));

            StringAssert.Contains("has invalid data format or has no sheetTable with 0 index.", ex.Message);

            File.Delete (file);
        }

        #endregion


        #region ImportData With Stream

        [Test]
        public void ImportData_TypeIsNull_Throws()
        {
            var stream = new MemoryStream(new byte[1]);
            var ex = Assert.Catch<ArgumentNullException>(() => ExcelImporter.ImportData(stream, null, 0));

            StringAssert.Contains("Type cannot be null.", ex.Message);
        }

        [Test]
        public void ImportData_TypeWithoutPublicParameterlessCtor_Throws()
        {
            var type = TypeExcelFactory.ClassWithParameterizedCtor;
            var stream = StreamFactory.GetExcelMemoryStream();
            var ex = Assert.Catch<TypeAccessException> (() => ExcelImporter.ImportData (stream, type, 0));

            StringAssert.Contains("has no public parameterless constructor!", ex.Message);
        }

        [Test]
        public void ImportData_SheetIndexIsNegative_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var stream = new MemoryStream(new byte[1]);
            var ex = Assert.Catch<ArgumentException>(() => ExcelImporter.ImportData(stream, type, -1));

            StringAssert.Contains("Index of sheet cannot be less than zero.", ex.Message);
        }

        [Test]
        public void ImportData_StreamDoesNotContainExcelData_Throws()
        {
            var type = TypeExcelFactory.EmptyClass;
            var stream = new MemoryStream(new byte[1]);
            var ex = Assert.Catch<FileFormatException>(() => ExcelImporter.ImportData(stream, type, 0));

            StringAssert.Contains("has invalid data format or has no sheetTable with 0 index.", ex.Message);
        }

        #endregion
    }
}
