using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute (string header)
        {
            Header = header;
        }

        public string Header { get; set; }
    }
}
