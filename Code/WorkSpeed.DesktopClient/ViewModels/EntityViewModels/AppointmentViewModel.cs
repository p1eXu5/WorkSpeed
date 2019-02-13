using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class AppointmentViewModel : ViewModel
    {
        private readonly Appointment _appintment;

        public AppointmentViewModel ( Appointment appintment )
        {
            _appintment = appintment;
        }
    }
}
