using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Contexts
{
    public class Service : IService
    {
        private bool _disposed;
        protected readonly WorkSpeedDbContext _dbContext;

        public WorkSpeedDbContext DbContext => _dbContext;

        public Service ( WorkSpeedDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public void Dispose ()
        {
            Dispose( true );
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed ) return;

            _dbContext.Dispose();
            _disposed = true;
        }
    }
}
