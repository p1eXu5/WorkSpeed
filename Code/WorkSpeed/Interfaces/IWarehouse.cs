﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Interfaces
{
    public interface IWarehouse
    {
        IWarehouseEntities NewData { get; }

        Task<bool> ImportAsync (string fileName);
        Task<bool> ImportAsync< TImportModel > ( string fileName ) where TImportModel : ImportModel;
        Task<bool> HasProductsAsync ();
    }
}
