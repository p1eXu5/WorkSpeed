using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WorkSpeed.Import.FileImporters;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    [SuppressMessage ("ReSharper", "UseMethodAny.2")]
    public class ImporterUnitTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-Us", false);
        }

        [Test]
        public void Importer_IsITypeReposytory()
        {
            Assert.IsTrue (typeof(ITypeRepository).IsAssignableFrom(typeof(Importer)));
        }

        [Test]
        public void RegisterFileImporter_ByDefault_AddsFileExtensionIntoDictionary()
        {
            // Arrange:
            var importer = GetImporter();
            var fileExtensions = new[] { ".xls", ".tst" };
            var fileImporter = GetFileImporter(fileExtensions);

            // Action:
            importer.RegisterFileImporter (fileImporter);

            // Assert:
            Assert.That (importer.FileExtensions, Is.EquivalentTo (fileExtensions));
        }

        [Test]
        public void RegisterFileImporter_FileImporterIsNull_Throws()
        {
            // Arrange:
            var importer = GetImporter();
            IFileImporter fileImporter = null;

            // Action:
            var ex = Assert.Catch<ArgumentNullException> (() => importer.RegisterFileImporter (fileImporter));

            // Assert:
            StringAssert.Contains ("fileImporter can't be null", ex.Message);
        }


        [Test]
        public void RegisterFileImporter_FileImporterHasNoFileExtensions_Throws()
        {
            // Arrange:
            var importer = GetImporter();
            var fileExtensions = new string[0];
            var fileImporter = GetFileImporter(fileExtensions);

            // Action:
            var ex = Assert.Catch<ArgumentException> (() => importer.RegisterFileImporter (fileImporter));

            // Assert:
            StringAssert.Contains ("IFileImporter instance does not have extensions", ex.Message);
        }

        [TestCase ("")]
        [TestCase (" ")]
        [TestCase ("xls")]
        public void RegisterFileImporter_FileImporterHasInvalidFileExtensions_Throws(string fileExtension)
        {
            // Arrange:
            var importer = GetImporter();
            var fileExtensions = new[] {fileExtension};
            var fileImporter = GetFileImporter(fileExtensions);

            // Action:
            var ex = Assert.Catch<ArgumentException> (() => importer.RegisterFileImporter (fileImporter));

            // Assert:
            StringAssert.Contains ("IFileImporter instance does not have valid extensions", ex.Message);
        }

        [Test]
        public void RegisterFileImporter_FileImporterContainsInvalidFileExtensions_AddsValidFileExtensionIntoDictionary()
        {
            // Arrange:
            var importer = GetImporter();
            var fileExtensions = new[] {"", ".xls", "tst", " ", ".xlsx", "dfgfdgdfgd"};
            var fileImporter = GetFileImporter(fileExtensions);

            // Action:
            importer.RegisterFileImporter (fileImporter);

            // Assert:
            Assert.That (importer.FileExtensions, Is.EquivalentTo (new[] {".xls", ".xlsx"}));
        }

        [Test]
        public void RegisterFileImporter_FileExtensionAlreadyInMap_ReplacesFileImporterAtFileExtensionInMap()
        {
            // Arrange:
            var importer = GetImporter();

            var mockFileImporter = GetMockFileImporter(".fake");
            importer.RegisterFileImporter(mockFileImporter.Object);

            // Action:
            var mockFileImporter2 = GetMockFileImporter(".fake");

            importer.RegisterFileImporter(mockFileImporter2.Object);
            importer.ImportData ("fake.fake");

            // Assert:
            mockFileImporter2.Verify(fi => fi.ImportData (It.IsAny<string>(), It.IsAny<ITypeRepository>()));
        }

        [Test]
        public void ImportData_ByDefault_CallsFileImporterAccordingToTheFileExtension()
        {
            // Arrange:
            var importer = GetImporter();
            var mockFileImporter = GetMockFileImporter(".fake");

            // Action:
            importer.RegisterFileImporter (mockFileImporter.Object);
            importer.ImportData ("fake.fake");

            // Assert:
            mockFileImporter.Verify(fi => fi.ImportData (It.IsAny<string>(), It.IsAny<ITypeRepository>()));
        }

        [Test]
        public void ImportData_ByDefault_PassesFileToTheFileImporter()
        {
            // Arrange:
            var importer = GetImporter();
            var mockFileImporter = GetMockFileImporter(".fake");

            // Action:
            importer.RegisterFileImporter(mockFileImporter.Object);
            importer.ImportData("fake.fake");

            // Assert:
            mockFileImporter.Verify(fi => fi.ImportData("fake.fake", It.IsAny<ITypeRepository>()));
        }

        [Test]
        public void EmployeesGetter_ByDefault_ReturnsEmptyCollection()
        {
            // Arrange:
            var importer = GetImporter();

            // Action:
            var employees = importer.Employees;

            // Assert:
            Assert.That (0 == employees.Count());
        }

        [Test]
        public void AddressesGetter_ByDefault_ReturnsEmptyCollection()
        {
            // Arrange:
            var importer = GetImporter();

            // Action:
            var addresses = importer.Addresses;

            // Assert:
            Assert.That(0 == addresses.Count());
        }

        [Test]
        public void PeriodGetter_ByDefault_ReturnsNull()
        {
            // Arrange:
            var importer = GetImporter();

            // Action:
            var period = importer.Period;

            // Assert:
            Assert.That(period, Is.Null);
        }


        #region Factory

        private Mock<IFileImporter> GetMockFileImporter(string fileExtension)
        {
            Mock<IFileImporter> mockFileImporter = new Mock<IFileImporter>();
            mockFileImporter.Setup (fi => fi.FileExtensions).Returns (new HashSet<string> { fileExtension });

            return mockFileImporter;
        }

        private Importer GetImporter()
        {
            return new Importer ();
        }

        private IFileImporter GetFileImporter (IEnumerable<string> fileExtaneions)
        {
            return new FakeFileImporter (fileExtaneions);
        }

        private class FakeFileImporter : IFileImporter
        {
            public FakeFileImporter (IEnumerable<string> fileExtansions)
            {
                FileExtensions = new HashSet<string>(fileExtansions);
            }

            public IEnumerable<string> FileExtensions { get; }
            public ICollection ImportData (string fileName, ITypeRepository typeRepository)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
