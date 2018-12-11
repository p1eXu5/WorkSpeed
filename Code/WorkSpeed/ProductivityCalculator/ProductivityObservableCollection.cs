using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Comparers;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.ProductivityCalculator
{
    public class ProductivityObservableCollection : ObservableCollection<Productivity>
    {
        private SortedSet<EmployeeAction> _actions;

        public ProductivityObservableCollection()
        {
            _actions = new SortedSet<EmployeeAction> (new EmployeeActionComparer<EmployeeAction>());
        }

        public IProductivityCalculator<EmployeeAction> ProductivityCalculator { get; set; }

        /// <summary>
        /// Adds action into internal collection. After adding
        /// Calculate method must be called.
        /// </summary>
        /// <param name="action">Imported action.</param>
        public void Add(ActionImportModel action)
        {
            _actions.Add (action.GetAction());
        }

        public void Calculate()
        {
            ProductivityCalculator.Calculate (_actions, this);
            
            _actions.Clear();
        }

        public IEnumerable<Employee> GetEmployees() => this.Select (p => p.Employee);
    }
}
