using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business
{
    public class CategoryConstraints : ICategoryConstraints
    {
        public int Count { get; }
        public int GetCategoryNum ( Product product )
        {
            throw new NotImplementedException();
        }
    }
}
