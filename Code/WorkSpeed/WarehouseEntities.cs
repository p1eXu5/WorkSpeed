using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed
{
    public class WarehouseEntities : IWarehouseEntities
    {
        private readonly IWorkSpeedDbContext _dbContext;
        private readonly IProductivityCalculator _productivityCalculator;

        private readonly ObservableCollection<Employee> _employees;
        private readonly ObservableCollection<Productivity> _productivity;

        public WarehouseEntities(IWorkSpeedDbContext dbContext,IProductivityCalculator productivityCalculator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException();
            _productivityCalculator = productivityCalculator ?? throw new ArgumentNullException();

            _employees = new ObservableCollection<Employee>();
            Employees = new ReadOnlyObservableCollection<Employee> (_employees);

            _productivity = new ObservableCollection<Productivity>();
            Employees = new ReadOnlyObservableCollection<Employee>(_employees);
        }

        public ReadOnlyObservableCollection<Employee> Employees { get; }
        public ReadOnlyObservableCollection<Employee> Productivities { get; }

        public void Add (IEnumerable<ImportModel> importModels)
        {
            if (importModels == null) throw new ArgumentNullException();

            var models = importModels.ToArray();

            Clear();

            foreach (var model in models) {

                var dbEmployee = _dbContext.GetEmployees().FirstOrDefault (e => e.Id == model.Employee.Id) ?? model.Employee;

                if (dbEmployee != null) {
                    _employees.Add (model.Employee);
                }

                _productivity.Add (_productivityCalculator.CalculatePoductivities(model.GetActions()));
            }
        }

        private void Clear()
        {
            _employees.Clear();
            _productivity.Clear();
        }
    }
}
