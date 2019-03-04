using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class AppointmentViewModel : AbbreviationViewModel
    {
        private readonly Appointment _appointment;

        public AppointmentViewModel ( Appointment appointment )
        {
            _appointment = appointment;
            Abbreviation = GetAbbreviation( appointment.Abbreviations );
        }

        public Appointment Appointment => _appointment;
        public string InnerName => _appointment.InnerName;
    }
}
