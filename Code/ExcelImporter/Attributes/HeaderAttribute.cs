using System;
using static Helpers.StringExtensions;

namespace ExcelImporter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute (string header)
        {
            Header = header.RemoveWhitespaces().ToUpperInvariant();
        }

        /// <summary>
        /// Normalized header namely without whitespases and in upper case.
        /// </summary>
        public string Header { get; set; }
    }
}
