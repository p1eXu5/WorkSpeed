using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data
{
    public interface IWorkSpeedData
    {
        IEnumerable<Employee> GetEmployees();
    }
}
