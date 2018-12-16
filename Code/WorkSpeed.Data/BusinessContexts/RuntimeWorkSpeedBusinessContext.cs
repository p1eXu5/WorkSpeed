using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public class RuntimeWorkSpeedBusinessContext : IWorkSpeedBusinessContext
    {
        private readonly List< Product > _products = new List< Product >( 40_000 );


        public Task< bool > HasProductsAsync ()
        {
            return Task.Factory.StartNew( () => false );
        }
    }
}
