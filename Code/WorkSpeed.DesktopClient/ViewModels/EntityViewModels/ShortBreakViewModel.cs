using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ShortBreakViewModel : ViewModel
    {
        private readonly ShortBreak _break;

        public ShortBreakViewModel ( ShortBreak shortBreak )
        {
            _break = shortBreak;
        }

        public int Id => _break.Id;

        public string Name
        {
            get => _break.Name;
            set {
                _break.Name = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get => _break.Duration;
            set {
                _break.Duration = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Interval
        {
            get => _break.Interval;
            set {
                _break.Interval = value;
                OnPropertyChanged();
            }
        }

        public bool IsForSmokers
        {
            get => _break.IsForSmokers;
            set {
                _break.IsForSmokers = value;
                OnPropertyChanged();
            }
        }

        public string ShiftName
        {
            get => _break.Shift.Name;
            set {
                _break.Shift.Name = value;
                OnPropertyChanged();
            }
        }
    }
}
