using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public interface IWorkSpeedBusinessContext
    {
        bool HasProducts();
        Task<bool> HasProductsAsync ();

        IEnumerable< Product > GetProducts();
        void AddProduct ( Product product );
    }
}
