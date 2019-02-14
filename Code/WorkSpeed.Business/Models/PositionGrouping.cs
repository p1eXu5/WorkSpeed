using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models
{
    public class PositionGrouping
    {
        public PositionGrouping ( Position position, Employee[] employees )
        {
            Position = position;
            Employees = employees;
        }

        public Position Position { get; }
        public Employee[] Employees { get; }
    }
}
