using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Interfaces
{
    public interface IWarehouseEntities
    {
        ReadOnlyObservableCollection<Employee> Employees { get; }
        ReadOnlyObservableCollection<Employee> Productivities { get; }

    }
}
