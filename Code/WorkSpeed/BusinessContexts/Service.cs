using System;
using WorkSpeed.Business.BusinessContexts.Contracts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Business.BusinessContexts
{
    public class Service : IService
    {
        private bool _disposed;

        public WorkSpeedDbContext WorkSpeedDbContext { get; }

        public Service ( WorkSpeedDbContext dbContext )
        {
            WorkSpeedDbContext = dbContext;
        }

        public void Dispose ()
        {
            throw new NotImplementedException();
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed ) return;

            WorkSpeedDbContext.Dispose();
            _disposed = true;
        }
    }
}
