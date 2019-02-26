﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public class EmployeeProductivity : IEmployeeProductivity
    {

        public EmployeeProductivity ( Employee employee, (IReadOnlyDictionary< Operation, IProductivity >, HashSet< Period > ) productivities )
        {
            Employee = employee;
        }

        public Employee Employee { get; set; }

        public IEnumerable< (double count, Operation operation) > GetTimes ( IEnumerable< Operation > operations )
        {
            throw new NotImplementedException();
        }

        public IProductivity this[ Operation operation ] => Productivities[ operation ];

        public IReadOnlyDictionary< Operation, IProductivity > Productivities { get; set; }

        public double GetTotalHours ()
        {
            throw new NotImplementedException();
        }
    }
}