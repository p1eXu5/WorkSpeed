using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public class WorkSpeedBusinessContext : IDisposable
    {
        private readonly WorkSpeedDataContext _dbContext;
        private bool _disposed;


        public WorkSpeedBusinessContext()
        {
            _dbContext = new WorkSpeedDataContext();
        }


        public bool HasProducts ()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasProductsAsync ()
        {
            return _dbContext.Products.AnyAsync();
        }

        public IEnumerable< Product > GetProducts ()
        {
            throw new NotImplementedException();
        }

        public void AddProduct ( Product product )
        {
            throw new NotImplementedException();
        }

        public ReadOnlyObservableCollection< GatheringAction > GatheringActions { get; }

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
