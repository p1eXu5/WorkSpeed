using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class EmployeeViewModel : ViewModel
    {
        private readonly Employee _employee;

        public EmployeeViewModel (Employee employee)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public Employee Employee => _employee;

        public string Id => _employee.Id;

        public Appointment Appointment
        {
            get => _employee.Appointment;
            set {
                _employee.Appointment = value;
                OnPropertyChanged();
            }
        }

        public string Name => _employee.Name;

        public bool IsActive
        {
            get => _employee.IsActive;
            set {
                _employee.IsActive = value;
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

        public Rank Rank
        {
            get => _employee.Rank;
            set {
                _employee.Rank = value;
                OnPropertyChanged();
            }
        }

        public bool IsSmoker
        {
            get => _employee.IsSmoker;
            set {
                _employee.IsSmoker = value;
                OnPropertyChanged();
            }
        }
    }
}
