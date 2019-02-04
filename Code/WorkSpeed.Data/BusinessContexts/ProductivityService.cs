using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.BusinessContexts
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
