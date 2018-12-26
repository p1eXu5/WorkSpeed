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
        private readonly List< Category > _categories;

        public CategoryFilter ( IEnumerable<Category> categories )
        {
            var inputCategories = categories.OrderBy( c => c.MaxVolume ).ThenBy( c => c.Date ).ToArray();

            var maxElement = inputCategories.FirstOrDefault( c => c.MaxVolume.Equals( 0.0 ) );
            int iStart = 0;

            if ( maxElement != null ) {
                maxElement.MaxVolume = Double.PositiveInfinity;
            }
            else {
                maxElement = new Category {
                    Date = DateTime.Now,
                    MaxVolume = Double.PositiveInfinity,
                    MinVolume = inputCategories[0].MaxVolume,
                    Name = $"Товары от { inputCategories[ 0 ].MaxVolume } литров"
                };
                iStart = 1;
            }

            _categories = new List< Category > { maxElement };

            for ( int i = iStart; i < inputCategories.Length; ++i ) {

                if ( inputCategories[ i ].MinVolume >= 0.0
                     && inputCategories[ i ].MaxVolume < maxElement.MinVolume ) {

                    _categories.Add( inputCategories[ i ] );
                    maxElement = inputCategories[ i ];
                }

                if ( inputCategories[ i ].MinVolume.Equals( 0.0 ) ) break;
            }
        }

        public int GetCategory ( Product product )
        {
            throw new NotImplementedException();
        }

        public string GetCategoryName ( int category )
        {
            throw new NotImplementedException();
        }

        public int Count => _categories.Count;
    }
}
