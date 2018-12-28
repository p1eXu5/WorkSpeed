using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class CategoryFilter : ICategoryFilter
    {
        private readonly List< Category > _categoryList;
        private readonly List< Category > _fillingCategoryList;

        public CategoryFilter ( IEnumerable<Category> categories )
        {
            if ( categories == null )
                throw new ArgumentNullException( nameof( categories ), "IEnumerable< CAtegory > cannot be null." );

            var categoriesArray = categories.ToArray();
            if ( !categoriesArray.Any() ) throw new ArgumentException();

            _categoryList = new List< Category >( categoriesArray.Count() );
            _fillingCategoryList = new List< Category >();

            foreach ( Category category in categoriesArray ) {
                AddCategory( category );
            }
        }


        public IEnumerable< Category > CategoryList => _categoryList;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory ( Category category )
        {
            CheckCategory( category );

            _categoryList.Add( category );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int GetCategoryIndex ( Product product )
        {
            if ( product == null ) throw new ArgumentNullException( nameof( product ), "Product cannot be null." );

            var volume = product.GetVolume();
            var category = _categoryList.FirstOrDefault( c => volume >= c.MinVolume && volume < c.MaxVolume );

            if ( null == category ) return -1;

            return _categoryList.IndexOf( category );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool ContainsVolume ( Category category )
        {
            CheckCategory( category );

            if ( _categoryList.Any( c => category.MinVolume >= c.MinVolume && category.MinVolume < c.MaxVolume  )
                || _categoryList.Any( (c => category.MaxVolume >=  c.MinVolume && category.MaxVolume < c.MaxVolume ) ) ) {

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillHoles ()
        {
            if ( !_categoryList.Any() ) {

                _categoryList.Add( new Category { MinVolume = 0, MaxVolume = double.PositiveInfinity } );
                _fillingCategoryList.Add( _categoryList.Last() );
                return;
            }

            var categories = _categoryList.OrderBy( c => c.MinVolume ).ToArray();

            if ( categories[ 0 ].MinVolume > 0 ) {
                _categoryList.Add( new Category { MinVolume = 0, MaxVolume = categories[ 0 ].MinVolume } );
                _fillingCategoryList.Add( _categoryList.Last() );
            }

            if ( categories[ categories.Length - 1 ].MaxVolume < double.PositiveInfinity ) {
                _categoryList.Add( new Category { MinVolume = categories[ categories.Length - 1 ].MaxVolume, MaxVolume = double.PositiveInfinity } );
                _fillingCategoryList.Add( _categoryList.Last() );
            }

            for ( int i = 1; i < categories.Length; ++i ) {

                if ( categories[ i - 1 ].MaxVolume < categories[ i ].MinVolume ) {
                    _categoryList.Add( new Category { MinVolume = categories[ i - 1 ].MaxVolume, MaxVolume = categories[ i ].MinVolume } );
                    _fillingCategoryList.Add( _categoryList.Last() );
                }
            }
        }

        public void UndoHoles ()
        {
            if ( !_fillingCategoryList.Any() ) throw new InvalidOperationException();

            foreach ( var category in _fillingCategoryList ) {
                _categoryList.Remove( category );
            }

            _fillingCategoryList.Clear();
        }

        public bool HasHoles ()
        {
            if ( _categoryList.Count == 1 ) {

                return !(_categoryList[ 0 ].MinVolume.Equals( 0.0 ) &&
                       _categoryList[ 0 ].MaxVolume.Equals( double.PositiveInfinity ));
            }

            var categories = _categoryList.OrderBy( c => c.MinVolume ).ToArray();

            if ( !categories[ 0 ].MinVolume.Equals( 0.0 ) ) {
                return true;
            }

            if ( !categories[ categories.Length - 1 ].MaxVolume.Equals( double.PositiveInfinity ) ) {
                return true;
            }

            for ( int i = 1; i < categories.Length; ++i )
            {

                if ( categories[ i - 1 ].MaxVolume < categories[ i ].MinVolume ) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        private void CheckCategory ( Category category )
        {
            if ( category == null ) throw new ArgumentNullException( nameof( category ) );

            if ( category.MaxVolume <= category.MinVolume
                 || category.MinVolume < 0
                 || category.MaxVolume < 0 ) throw new ArgumentException();
        }
    }
}
