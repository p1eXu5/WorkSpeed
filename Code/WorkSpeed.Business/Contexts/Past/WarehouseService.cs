using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Contexts
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
