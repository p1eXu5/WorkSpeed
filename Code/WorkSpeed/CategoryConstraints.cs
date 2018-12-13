using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed
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
