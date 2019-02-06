using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Constraints
{
    public class DefaultCategoryConstraints : ICategoryConstraints
    {
        public int Count { get; }
        public int GetCategoryNum ( Product product )
        {
            throw new NotImplementedException();
        }
    }
}
