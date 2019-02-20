using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Entities
{
    public class ShortBreakScheduleViewModel : ViewModel
    {
        private readonly ShortBreakSchedule _shortBreakSchedule;

        public ShortBreakScheduleViewModel ( ShortBreakSchedule shortBreakSchedule )
        {
            _shortBreakSchedule = shortBreakSchedule;
        }

        public ShortBreakSchedule ShortBreakSchedule => _shortBreakSchedule;

        public string Name
        {
            get => _shortBreakSchedule.Name;
            set {
                _shortBreakSchedule.Name = value;
                OnPropertyChanged();
            }
        }
    }
}
