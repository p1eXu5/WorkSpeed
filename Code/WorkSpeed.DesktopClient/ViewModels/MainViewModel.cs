﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public DataImportViewModel DataImportViewModel { get; set; } = new DataImportViewModel();
    }
}
