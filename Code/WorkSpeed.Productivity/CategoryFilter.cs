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
            CategoryList = new List< Category >( categories.Count() + 4 );
        }

        public List< Category > CategoryList { get; }

        public void AddCategory ( Category category )
        {
            throw new NotImplementedException();
        }

        public int GetCategoryIndex ( Product product )
        {
            throw new NotImplementedException();
        }

        public bool Contains ( Category category )
        {
            throw new NotImplementedException();
        }

        public void FillHoles ()
        {
            throw new NotImplementedException();
        }
    }
}
