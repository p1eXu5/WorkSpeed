using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public abstract class ProductivityIndicators
    {
        protected ProductivityIndicators ( string name )
        {
            Name = name ?? throw new ArgumentNullException();
        }

        public string Name { get; }

        protected abstract void Add ( EmployeeAction employeeAction );
    }
}
