using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel (Employee employee)
        {
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public Employee Employee { get; }
    }
}
