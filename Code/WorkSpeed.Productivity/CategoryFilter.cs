using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data;
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

                if ( !inputCategories[ i ].MaxVolume.Equals( 0.0 ) ) {

                    if ( maxElement.MinVolume.Equals( null )
                        && inputCategories[ i ].MaxVolume < maxElement.MaxVolume
                        ) {

                        maxElement.MinVolume = inputCategories[ i ].MaxVolume;
                        _categories.Add( inputCategories[ i ] );
                        maxElement = inputCategories[ i ];
                    }
                    else if ( inputCategories[ i ].MaxVolume <= maxElement.MinVolume ) {
                        _categories.Add( inputCategories[ i ] );
                        maxElement = inputCategories[ i ];
                    }
                }

                if ( inputCategories[ i ].MinVolume.Equals( 0.0 ) ) break;
            }

            if ( maxElement.MinVolume > 0 ) {

                _categories.Add( new Category {

                    Date = DateTime.Now,
                    MaxVolume = (double)maxElement.MinVolume,
                    MinVolume = 0,
                    Name = $"Товары до { maxElement.MaxVolume } литров"
                } );
            }
        }

        public int GetCategoryIndex ( Product product )
        {
            var productVolume = product.GetVolume();
            var category = _categories.First( c => productVolume < c.MaxVolume && productVolume >= c.MinVolume  );
            return _categories.IndexOf( category );
        }

        public string GetCategoryName ( int index )
        {
            if ( index < _categories.Count ) {
                return _categories[ index ].Name;
            }

            return null;
        }

        public int Count => _categories.Count;
    }
}
