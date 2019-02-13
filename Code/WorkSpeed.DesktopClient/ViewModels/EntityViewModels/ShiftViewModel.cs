using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ShiftViewModel : ViewModel
    {
        private readonly Shift _shift;

        public ShiftViewModel ( Shift shift )
        {
            _shift = shift;
        }


    }
}
