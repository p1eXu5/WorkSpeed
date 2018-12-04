using System;
using WorkSpeed.Import;

namespace ExcelImporter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute (string header)
        {
            Header = header.RemoveWhitespaces();
        }

        public string Header { get; set; }
    }
}
