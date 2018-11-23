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

            var ex = Assert.Catch<TypeAccessException>(() => importer.ImportData<FakeModelClassWithNoPublicProperties>
                                                       (
                                                           Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\TestFiles\test.xlsx")
                                                       );

            StringAssert.Contains("Passed type does not have public properties", ex.Message);
        }

        [Test]
        public void ImportData_UnhandledDataSourse_Throw()
        {
            var importer = GetDataImporter();

            var ex = Assert.Catch<ArgumentException>(() => importer.ImportData<FakeModelClass>
                                                           (
                                                            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\TestFiles\test")
                                                      );

            StringAssert.Contains("The source does not handled", ex.Message);
        }


        #region Factory

            private IDataImporter GetDataImporter()
        {
            return new DataImporter();
        }

        class FakeModelClassWithNoPublicProperties
        { }

        class FakeModelClass
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
