
using System.Collections.ObjectModel;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Interfaces
{
    public interface IWarehouseEntities
    {
        ReadOnlyObservableCollection<Employee> Employees { get; }
        ReadOnlyObservableCollection<Employee> Productivities { get; }

    }
}
