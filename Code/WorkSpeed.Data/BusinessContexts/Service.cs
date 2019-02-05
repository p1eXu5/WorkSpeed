using System;
using WorkSpeed.Data.BusinessContexts.Contracts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.BusinessContexts
{
    public class Service : IService
    {
        private bool _disposed;

        public WorkSpeedDbContext DdContext { get; }

        public Service ( WorkSpeedDbContext dbContext )
        {
            DdContext = dbContext;
        }

        public void Dispose ()
        {
            throw new NotImplementedException();
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed ) return;

            DdContext.Dispose();
            _disposed = true;
        }
    }
}
