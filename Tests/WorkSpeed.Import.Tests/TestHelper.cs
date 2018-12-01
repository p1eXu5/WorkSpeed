using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Tests
{
    internal static class TestHelper
    {
        internal static string GetFullPath (this string fileName)
        {
            return new StringBuilder().Append (Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly().Location))
                                      .Append (@"\TestFiles\")
                                      .Append (fileName).ToString();
        }


        internal static string CreateFakeEmptyTestFile (string fileName)
        {
            using (var stream = new FileStream (fileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                return stream.Name;
            }
        }
    }
}
