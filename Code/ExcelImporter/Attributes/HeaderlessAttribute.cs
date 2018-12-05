﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImporter.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class HeaderlessAttribute : Attribute
    {
        public bool IsHeadless { get; set; } = true;
    }
}