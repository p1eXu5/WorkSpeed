using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Import.Tests.UnitTests
{
    public class DataImporterUnitTests
    {
        [Test]
        public void ImportData_TypeWithoutPublicProperties_Throw()
        {
            var importer = GetDataImporter();

            var ex = Assert.Catch<TypeAccessException>( () => importer.ImportData<FakeModelClassWithNoPublicProperties> ( GetFullPath("test.xlsx")) );

            StringAssert.Contains("Passed type does not have public properties", ex.Message);
        }

        [Test]
        public void ImportData_UnhandledDataSourse_Throw()
        {
            var importer = GetDataImporter();

            var ex = Assert.Catch<ArgumentException>( () => importer.ImportData<FakeModelClass> ( GetFullPath("test")) );

            StringAssert.Contains("The source does not handled", ex.Message);
        }

        [Test]
        public void ImportData_XlsxFileDoesntContainData_ReturnsEmptyCollection()
        {
            // Arrange:
            var importer = GetDataImporter();

            // Action:
            var resColl = importer.ImportData<FakeModelClass>(GetFullPath ("empty.xlsx"));

            // Assert:
            Assert.That (0 == resColl.Count);
        }


        #region Factory

        private IDataImporter GetDataImporter()
        {
            return new DataImporter();
        }

        private string GetFullPath(string fileName) => new StringBuilder()
                                                            .Append ( Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly().Location) )
                                                            .Append (@"\TestFiles\")
                                                            .Append (fileName).ToString();

        class FakeModelClassWithNoPublicProperties
        { }

        class FakeModelClass
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
