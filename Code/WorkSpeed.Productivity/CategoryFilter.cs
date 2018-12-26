using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class CategoryFilter : ICategoryFilter
    {
        public CategoryFilter ( IEnumerable<Category> categories )
        {
            
        }

        public int GetCategory ( Product product )
        {
            throw new NotImplementedException();
        }

        public string GetCategoryName ( int category )
        {
            throw new NotImplementedException();
        }

        public int Count { get; }
    }
}
