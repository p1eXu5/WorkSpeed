using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class HeadlessAttribute : Attribute
    {
        public bool IsHeadless { get; set; } = true;
    }
}
