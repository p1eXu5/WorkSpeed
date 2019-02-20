using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public class EmployeeProductivityBase
    {
        private readonly Dictionary< Operation, IProductivity > _productivities;

        public EmployeeProductivityBase ( Employee employee )
        {
            Employee = employee;
            _productivities = new Dictionary< Operation, IProductivity >();
        }

        public Employee Employee { get; set; }

        public IProductivity this[ Operation operation ]
        {
            get => _productivities[ operation ];
            set => _productivities[ operation ] = value;
        }

        public void AddRange ( IEnumerable< IProductivity > productivities )
        {
            foreach ( var productivity in productivities ) {

                _productivities[ productivity.Operation ] = productivity;
            }
        }
    }
}
