﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Data.Comparers;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.ProductivityCalculator
{
    public class ProductivityObservableCollection : ObservableCollection< Productivity2 >
    {
        private SortedSet< EmployeeActionBase > _actions;

        public ProductivityObservableCollection()
        {
            _actions = new SortedSet< EmployeeActionBase > (new EmployeeActionComparer< EmployeeActionBase >());
        }

        public IProductivityCalculator< EmployeeActionBase > ProductivityCalculator { get; set; }

        /// <summary>
        /// Adds employeeAction into internal collection. After adding
        /// Calculate method must be called.
        /// </summary>
        /// <param name="employeeAction">Imported employeeAction.</param>
        public void Add( ActionImportModel employeeAction )
        {

        }

        public void Calculate()
        {
            ProductivityCalculator.Calculate (_actions, this);
            
            _actions.Clear();
        }

        public IEnumerable<Employee> GetEmployees() => this.Select (p => p.Employee);
    }
}
