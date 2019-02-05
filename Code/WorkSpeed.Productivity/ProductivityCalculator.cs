using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Productivity
{
    public class ProductivityCalculator : IProductivityCalculator, IDisposable
    {
        private bool _disposed;

        public ProductivityCalculator ()
        {
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

            _disposed = true;
        }
    }
}
