﻿using System;
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
        private readonly ShortBreakSchedule _break;

        public ShortBreakViewModel ( ShortBreakSchedule shortBreak )
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
            get => _break.Periodicity;
            set {
                _break.Periodicity = value;
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

        public TimeSpan DayOffsetTime
        {
            get => _break.DayOffsetTime;
            set {
                _break.DayOffsetTime = value;
                OnPropertyChanged();
            }
        }
    }
}
