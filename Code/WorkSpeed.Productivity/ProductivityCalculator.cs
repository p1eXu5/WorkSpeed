using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Productivity
{
    public class ProductivityCalculator : IProductivityCalculator, IDisposable
    {
        private readonly ProductivityService _context;
        private bool _disposed;

        public ProductivityCalculator ()
        {
            _context = new ProductivityService();
        }


        public IEnumerable< Productivity > CalculateProductivity ()
        {
            throw new NotImplementedException();
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
