using System;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Contexts
{
    public class ProductivityService : IDisposable
    {
        private readonly WorkSpeedDbContext _context;
        private bool _disposed;

        public ProductivityService ()
        {
            _context = new WorkSpeedDbContext();
        }


        public void Dispose ()
        {
            Dispose( true );
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed ) return;

            _context?.Dispose();
            _disposed = true;
        }
    }
}
