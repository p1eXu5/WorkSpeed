using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImporter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class HiddenAttribute : Attribute
    {
    }
}
