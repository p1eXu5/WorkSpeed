using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public class WorkSpeedBusinessContext : IWorkSpeedBusinessContext, IDisposable
    {
        private readonly WorkSpeedDataContext _dbContext;
        private bool _disposed;


        public WorkSpeedBusinessContext()
        {
            _dbContext = new WorkSpeedDataContext();
        }


        public Task<bool> HasProductsAsync ()
        {
            return _dbContext.Products.AnyAsync();
        }


        #region IDisposable

        private void Dispose (bool disposing)
        {
            if (!disposing && _disposed) {
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
