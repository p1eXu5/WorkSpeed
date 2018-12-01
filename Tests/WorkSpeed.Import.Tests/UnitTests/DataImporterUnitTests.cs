using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static WorkSpeed.Import.Tests.TestHelper;

namespace WorkSpeed.Import.Tests.UnitTests
{
    public class DataImporterUnitTests
    {
        [Test]
        public void ImportData_TypeWithoutPublicProperties_Throw()
        {


        }

        [Test]
        public void ImportData_UnhandledDataSourse_Throw()
        {

        }




        #region Factory

        

        class FakeModelClassWithNoPublicProperties
        { }

        class FakeModelClass
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
