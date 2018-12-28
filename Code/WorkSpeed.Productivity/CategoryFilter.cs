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
            if ( categories == null )
                throw new ArgumentNullException( nameof( categories ), "IEnumerable< CAtegory > cannot be null." );

            var categoriesArray = categories.ToArray();
            if ( !categoriesArray.Any() ) throw new ArgumentException();


            CategoryList = new List< Category >( categoriesArray.Count() + 4 );
        }

        public List< Category > CategoryList { get; }

        public virtual void AddCategory ( Category category )
        {
            throw new NotImplementedException();
        }

        public int GetCategoryIndex ( Product product )
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains ( Category category )
        {
            throw new NotImplementedException();
        }

        public virtual void FillHoles ()
        {
            if ( !CategoryList.Any() ) {

                CategoryList.Add( new Category { MinVolume = 0, MaxVolume = double.PositiveInfinity } );
                return;
            }


        }
    }
}
