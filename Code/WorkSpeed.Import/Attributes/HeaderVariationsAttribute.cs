using System;
using System.Linq;

namespace WorkSpeed.Import.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    class HeaderVariationsAttribute : Attribute
    {
        public HeaderVariationsAttribute (params string[] headers)
        {
            Headers = headers.Select (h => h.RemoveWhitespaces()).ToArray();
        }

        public string[] Headers { get; set; }
    }
}
