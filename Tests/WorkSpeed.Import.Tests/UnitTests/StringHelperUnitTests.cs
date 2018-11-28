using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    public class StringHelperUnitTests
    {
        [TestCase ("", "")]
        [TestCase (" ", "")]
        [TestCase ("asd", "asd")]
        [TestCase (" aa agfdsfgdf.gknsflgsf;gnklsd;fga_ aa$ aa% aa)\n  ()  \t", "aaagfdsfgdf.gknsflgsf;gnklsd;fga_aa$aa%aa)()")]
        public void RemoveWhitespaces2_ByDefault_ReturnsStringWithoutWhitespaces(string inputString, string expectedString)
        {
            Assert.That (inputString.RemoveWhitespaces(), Is.EqualTo (expectedString));
        }
    }
}
