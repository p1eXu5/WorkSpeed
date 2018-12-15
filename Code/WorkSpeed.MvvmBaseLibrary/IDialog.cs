﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkSpeed.MvvmBaseLibrary
{
    public interface IDialog
    {
        Window Owner { get; set; }
        object DataContext { get; set; }
        bool? ShowDialog ();
        bool? DialogResult { get; set; }
    }
}
