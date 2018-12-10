using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Interfaces
{
    public interface IWarehouse
    {
        IWarehouseEntities NewData { get; }

        Task ImportAsync (string fileName);
    }
}
