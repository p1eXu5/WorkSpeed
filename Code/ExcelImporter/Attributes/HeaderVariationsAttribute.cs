using System;
using System.Linq;
using WorkSpeed.Import;

namespace ExcelImporter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    class HeaderVariationsAttribute : Attribute
    {
        public HeaderVariationsAttribute (params string[] headers)
        {
            Headers = headers.Select (h => h.RemoveWhitespaces()).ToArray();
        }

        public string[] Headers { get; set; }
    }
}
