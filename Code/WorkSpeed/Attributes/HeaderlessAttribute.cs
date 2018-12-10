using System;

namespace WorkSpeed.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class HeaderlessAttribute : Attribute
    {
        public bool IsHeadless { get; set; } = true;
    }
}
