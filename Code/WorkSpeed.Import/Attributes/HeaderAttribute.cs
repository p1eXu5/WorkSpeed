using System;

namespace WorkSpeed.Import.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute (string header)
        {
            Header = header.RemoveWhitespaces();
        }

        public string Header { get; set; }
    }
}
