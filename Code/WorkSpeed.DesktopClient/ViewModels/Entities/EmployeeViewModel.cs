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

namespace WorkSpeed.DesktopClient.ViewModels.Entities
{
    public class EmployeeViewModel : ViewModel
    {
        private readonly Employee _employee;

        public EmployeeViewModel(Employee employee)
        {
            _employee = employee ?? throw new ArgumentNullException( nameof( employee ), @"Employee can not be null." );

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

        public string EmployeeId => "dfgdf";

        public Appointment Appointment
        {
            get => _employee.Appointment;
            set {
                _employee.Appointment = value;
                OnPropertyChanged();
            }
        }

        public Rank Rank
        {
            get => _employee.Rank;
            set {
                _employee.Rank = value;
                OnPropertyChanged();
            }
        }

        public Position Position
        {
            get => _employee.Position;
            set {
                _employee.Position = value;
                OnPropertyChanged();
            }
        }

        public Shift Shift
        {
            get => _employee.Shift;
            set {
                _employee.Shift = value;
                OnPropertyChanged();
            }
        }

        public ShortBreakSchedule ShortBreakSchedule
        {
            get => _employee.ShortBreakSchedule;
            set {
                _employee.ShortBreakSchedule = value;
                OnPropertyChanged();
            }
        }


        public bool IsActive
        {
            get => _employee.IsActive;
            set {
                _employee.IsActive = value;
                OnPropertyChanged();
            }
        }


        public bool? IsSmoker
        {
            get => _employee.IsSmoker;
            set {
                _employee.IsSmoker = value;
                OnPropertyChanged();
            }
        }
    }
}
