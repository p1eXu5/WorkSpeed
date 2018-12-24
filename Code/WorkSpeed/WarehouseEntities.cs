using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorkSpeed.Data;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Comparers;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed
{
    public class WarehouseEntities : IWarehouseEntities
    {
        private readonly IWorkSpeedBusinessContext _dbContext;
        //private readonly IProductivityCalculator _productivityCalculator;

        private readonly ObservableCollection<Employee> _employees;
        private readonly ObservableCollection<Productivity2> _productivity;

        public WarehouseEntities(IWorkSpeedBusinessContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException();
            //_productivityCalculator = productivityCalculator ?? throw new ArgumentNullException();

            _employees = new ObservableCollection<Employee>();
            Employees = new ReadOnlyObservableCollection<Employee> (_employees);

            _productivity = new ObservableCollection<Productivity2>();
            Employees = new ReadOnlyObservableCollection<Employee>(_employees);
        }

        public ReadOnlyObservableCollection<Employee> Employees { get; }
        public ReadOnlyObservableCollection<Employee> Productivities { get; }




        private void Clear()
        {
            _employees.Clear();
            _productivity.Clear();
        }
    }
}
