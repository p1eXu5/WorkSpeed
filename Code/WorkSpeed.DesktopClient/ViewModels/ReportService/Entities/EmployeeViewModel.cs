using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class EmployeeViewModel : ViewModel
    {
        private readonly Employee _employee;
        private readonly Employee _original;
        private bool _isModify;

        public EmployeeViewModel(Employee employee)
        {
            _employee = employee ?? throw new ArgumentNullException( nameof( employee ), @"Employee can not be null." );

            _original = new Employee {
                Appointment = _employee.Appointment,
                Rank = _employee.Rank,
                Position = _employee.Position,
                Shift = _employee.Shift,
                ShortBreakSchedule = _employee.ShortBreakSchedule,
                IsSmoker = _employee.IsSmoker,
                IsActive = _employee.IsActive
            };

            var name = _employee.Name.Split( new[] { ' ' } );
            SecondName = name[ 0 ];
            FirstMiddleName = name.Length >= 3 
                                  ? $"{name[ 1 ]} {name[ 2 ]}" 
                                  : $"{name[ 1 ]} ";
        }

        public Employee Employee => _employee;

        public string SecondName { get; }
        public string FirstMiddleName { get; }

        public Avatar Avatar => _employee.Avatar;

        public string EmployeeId => _employee.Id;

        public bool IsModify
        {
            get => _isModify;
            set {
                _isModify = value;
                OnPropertyChanged();
            }
        }

        public Appointment Appointment
        {
            get => _employee.Appointment;
            set {
                _employee.Appointment = value;
                OnPropertyChanged();
                CheckModify();;
            }
        }

        public Rank Rank
        {
            get => _employee.Rank;
            set {
                _employee.Rank = value;
                OnPropertyChanged();
                CheckModify();;
            }
        }

        public Position Position
        {
            get => _employee.Position;
            set {
                _employee.Position = value;
                OnPropertyChanged();
                CheckModify();;
            }
        }

        public Shift Shift
        {
            get => _employee.Shift;
            set {
                _employee.Shift = value;
                OnPropertyChanged();
                CheckModify();;
            }
        }

        public ShortBreakSchedule ShortBreakSchedule
        {
            get => _employee.ShortBreakSchedule;
            set {
                _employee.ShortBreakSchedule = value;
                OnPropertyChanged();
                CheckModify();;
            }
        }


        public bool IsActive
        {
            get => _employee.IsActive;
            set {
                _employee.IsActive = value;
                OnPropertyChanged();
                CheckModify();
            }
        }

        public bool IsNotActive
        {
            get => !IsActive;
            set {
                IsActive = !value;
                OnPropertyChanged();
            }
        }

        public bool? IsSmoker
        {
            get => _employee.IsSmoker;
            set {
                _employee.IsSmoker = value;
                OnPropertyChanged();
                CheckModify();
            }
        }

        private void CheckModify ()
        {
            IsModify = _employee.IsSmoker != _original.IsSmoker
                       || _employee.IsActive != _original.IsActive
                       || _employee.ShortBreakSchedule != _original.ShortBreakSchedule
                       || _employee.Shift != _original.Shift
                       || _employee.Position != _original.Position
                       || _employee.Rank != _original.Rank
                       || _employee.Appointment != _original.Appointment;
        }
    }
}
