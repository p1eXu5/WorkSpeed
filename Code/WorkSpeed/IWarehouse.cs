using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed
{
    public interface IWarehouse: IWarehouseEntities
    {
        IWarehouseEntities NewData { get; }

        void ImportAsync (string fileName);
    }
}
