using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class CategoryFilter : ICategoryFilter
    {

        public CategoryFilter ( IEnumerable<Category> categories )
        {

        }

        public List< Category > CategoryList { get; }

        public void AddCategory ( Category category )
        {
            throw new NotImplementedException();
        }
    }
}
