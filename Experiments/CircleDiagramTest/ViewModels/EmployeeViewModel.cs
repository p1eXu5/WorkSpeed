using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class EmployeeViewModel
    {
        private readonly Employee _employee;

        public EmployeeViewModel ( Employee employee )
        {
            _employee = employee;
        }

        public string Name => _employee.Name;
    }
}
