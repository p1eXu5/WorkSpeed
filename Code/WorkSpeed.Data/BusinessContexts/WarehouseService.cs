using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.BusinessContexts
{
    public class WarehouseService : IDisposable, IWarehouseService
    {
        private readonly WorkSpeedDbContext _dbContext;
        private bool _disposed;


        public WarehouseService()
        {
            _dbContext = new WorkSpeedDbContext();
        }




        public Task ImportAsync ( string fileName )
        {
            throw new NotImplementedException();
        }


        #region IDisposable

        private void Dispose (bool disposing)
        {
            if (!disposing || _disposed) {
                return;
            }

            _dbContext?.Dispose();

            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose (true);
        }

        #endregion
    }
}
