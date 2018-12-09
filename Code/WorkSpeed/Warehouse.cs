using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed
{
    public class Warehouse : IWarehouse
    {
        public ReadOnlyObservableCollection<Employee> Employees { get; }
        public IWarehouseEntities NewData { get; }

        public async void ImportAsync (string fileName)
        {
            await Import(fileName);
        }

        private Task Import (string fileName)
        {

        }
    }
}
