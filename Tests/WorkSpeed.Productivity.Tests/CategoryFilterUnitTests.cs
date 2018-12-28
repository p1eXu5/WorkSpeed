using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.Tests
{
    [ TestFixture ]
    public class CategoryFilterUnitTests
    {
        [SetUp]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }


        #region Ctor

        [Test]
        public void Ctor_CategoriesIsNull_Throws ()
        {
            List< Category > list = null;

            Assert.Catch< ArgumentNullException >( () => new CategoryFilter( list ) );
        }

        [ Test ]
        public void Ctor__Categories_Empty__Throws ()
        {
            List< Category > list = new List< Category >();

            Assert.Catch<ArgumentException>( () => new CategoryFilter( list ) );
        }

        [Test]
        public void Ctor__Categories_NotEmpty__CallsContains ()
        {
            // Arrange:
            List<Category> list = new List<Category>();
            list.Add( new Category( 0, 10 ) );

            // Action:
            var fakeFilter = new FakeCategoryFilter( list );

            // Assert:
            Assert.That( true == fakeFilter.ConteinsCalled );
        }

        [Test]
        public void Ctor__Categories_NotEmpty__CallsAddCategory ()
        {
            // Arrange:
            List<Category> list = new List<Category>();
            list.Add( new Category( 0, 10 ) );

            // Action:
            var fakeFilter = new FakeCategoryFilter( list );

            // Assert:
            Assert.That( true == fakeFilter.AddCategoryCalled );
        }

        [Test]
        public void Ctor__Categories_NotEmpty__DoesNotCallFillHoles ()
        {
            // Arrange:
            List<Category> list = new List<Category>();
            list.Add( new Category( 0, 10 ) );

            // Action:
            var fakeFilter = new FakeCategoryFilter( list );

            // Assert:
            Assert.That( false == fakeFilter.FillHolesCalled );
        }

        [Test]
        public void Ctor__Categories_NotEmpty__CreatesNotEmptyCategoryList ()
        {
            List<Category> list = new List<Category>();
            list.Add( new Category( 0, 10 ) );

            Assert.That( 0 < new CategoryFilter( list ).CategoryList.Count );
        }

        #endregion


        #region Contains

        [Test]
        public void Contains__Category_IsNull__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );

            // Action:
            // Assert:
            Assert.Catch< ArgumentNullException >( () => filter.Contains( null ) );
        }

        [Test]
        public void Contains__Category_MinVolumeLargerMaxVolume__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var categoryWithMaxVolumeLessMinVolume = new Category( 10, 0 );

            // Action:
            // Assert:
            Assert.Catch<ArgumentException>( () => filter.Contains( categoryWithMaxVolumeLessMinVolume ) );
        }

        [Test]
        public void Contains__Category_NegativeMinVolume__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var categoryWithNegativeMinVolume = new Category( -10, 10 );

            // Action:
            // Assert:
            Assert.Catch<ArgumentException>( () => filter.Contains( categoryWithNegativeMinVolume ) );
        }

        [Test]
        public void Contains__Category_NegativeMaxVolume__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var categoryWithNegativeMaxVolume = new Category( -10, -1 );

            // Action:
            // Assert:
            Assert.Catch<ArgumentException>( () => filter.Contains( categoryWithNegativeMaxVolume ) );
        }

        [Test]
        public void Contains__Category_MinVolumeEqualsMaxVolume__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var categoryMinVolumeEqualsMaxVolume = new Category( 5, 5 );

            // Action:
            // Assert:
            Assert.Catch<ArgumentException>( () => filter.Contains( categoryMinVolumeEqualsMaxVolume ) );
        }

        [ TestCase( 5, 10) ]
        [ TestCase( 15, 19) ]
        [ TestCase( 15, 20) ]
        public void Contains__Category_VolumeInCategoryList__ReturnsTrue ( int min, int max )
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var containedCategory = new Category( min, max );

            // Action:
            // Assert:
            Assert.That( true == filter.Contains( containedCategory ) );
        }

        [TestCase( 0, 9 )]
        [TestCase( 21, 15 )]
        public void Contains__Category_VolumeNotInCategoryList__ReturnsFalse ( int min, int max )
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var containedCategory = new Category( min, max );

            // Action:
            // Assert:
            Assert.That( false == filter.Contains( containedCategory ) );
        }

        #endregion


        #region FillHoles

        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroMinVolumeCategory ()
        {
            // Arrange:
            var list = new List< Category > { new Category( 10, 20) };

            // Action:
            var filter = new CategoryFilter( list );
            var zeroMinVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MinVolume.Equals( 0.0 ) );

            // Assert:
            Assert.That( zeroMinVolumeCategory, Is.Not.Null );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroIdCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            // Action:
            var filter = new CategoryFilter( list );
            var zeroMinVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MinVolume.Equals( 0.0 ) );

            // Assert:
            Assert.That( 0 == zeroMinVolumeCategory?.Id );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesPositiveInfiniteMaxVolumeCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            // Action:
            var filter = new CategoryFilter( list );
            var zeroMaxVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MaxVolume.Equals( double.PositiveInfinity ) );

            // Assert:
            Assert.That( zeroMaxVolumeCategory, Is.Not.Null );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesZeroIdCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            // Action:
            var filter = new CategoryFilter( list );
            var zeroMaxVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MinVolume.Equals( double.PositiveInfinity ) );

            // Assert:
            Assert.That( 0 == zeroMaxVolumeCategory?.Id );
        }

        [Test]
        public void FillHoles__CategoryList_WithVolumeHole__CreatesFillingCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ), new Category( 30, 40) };

            // Action:
            var filter = new CategoryFilter( list );
            var fillingCategory = filter.CategoryList.FirstOrDefault( c => c.MaxVolume.Equals( 30.0 ) );

            // Assert:
            Assert.That( fillingCategory, Is.Not.Null );
            Assert.That( fillingCategory.MinVolume.Equals( 20.0 ) );
        }

        [Test]
        public void FillHoles__CategoryList_WithVolumeHole__CreatesZeroIdFillingCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ), new Category( 30, 40 ) };

            // Action:
            var filter = new CategoryFilter( list );
            var fillingCategory = filter.CategoryList.FirstOrDefault( c => c.MaxVolume.Equals( 30.0 ) );

            // Assert:
            Assert.That( 0 == fillingCategory?.Id );
        }

        #endregion


        #region AddCategory

        [Test]
        public void AddCategory__Category_IsNull__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            var filter = new CategoryFilter( list );

            Category nullCategory = null;

            // Action:
            // Assert:
            Assert.Catch< ArgumentNullException >( () => filter.AddCategory( nullCategory ) );
        }

        [Test]
        public void AddCategory__Category_IsNotNull__CallsContains ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            var fakeFilter = new FakeCategoryFilter( list );
            fakeFilter.ResetContainsCalled();

            var category = new Category( 5, 10 );

            // Action:
            fakeFilter.AddCategory( category );

            // Assert:
            Assert.That( fakeFilter.ConteinsCalled );
        }

        [Test]
        public void AddCategory__Contains_ReturnsTrue___Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };

            var fakeFilter = new FakeCategoryFilter( list );
            var category = new Category( 10, 20 );

            // Action:
            // Assert:
            Assert.Catch< InvalidOperationException >( () => fakeFilter.AddCategory( category ) );
        }

        [Test]
        public void AddCategory__Category_NullName__CreatesCategoryWithNullName ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Name = null };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.CategoryList[0].Name, Is.Null );
        }

        [Test]
        public void AddCategory__Category_NotNullName__CreatesCategoryWithNotNullName ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Name = "" };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.CategoryList[ 0 ].Name, Is.Not.Null );
        }

        [Test]
        public void AddCategory__Category_Id__CreatesCategoryEqualsId ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Id = 5 };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( 5 == filter.CategoryList[ 0 ].Id );
        }

        [Test]
        public void AddCategory__Category_Date__CreatesCategoryEqualsDate ()
        {
            // Arrange:
            var date = DateTime.Now;
            var category = new Category( 10, 20 ) { Date = date };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.CategoryList[ 0 ].Date.Equals( date ) );
        }

        #endregion


        #region CategoryList

        [Test]
        public void CategoryList__ByDefault__ReturnsNotEmptyCategories ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Id = 5 };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.CategoryList.Any() );
        }

        [Test]
        public void CategoryList__Clear__DoesNotClearCategoryList ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Id = 5 };
            var list = new List<Category> { category };
            var filter = new CategoryFilter( list );

            // Action:
            filter.CategoryList.Clear();

            // Assert:
            Assert.That( filter.CategoryList.Any() );
        }

        #endregion


        #region GetCategoryIndex

        [Test]
        public void GetCategoryIndex__Product_IsNull__Throws ()
        {
            // Arrange:
            var category = new Category( 10, 20 );
            var list = new List<Category> { category };

            var filter = new CategoryFilter( list );

            // Action:
            // Assert:
            Assert.Catch< ArgumentNullException >( () => filter.GetCategoryIndex( null ) );
        }

        [Test]
        public void GetCategoryIndex__Product_VolumeIsContainedInCategoryList__ReturnsCategoryIngex ()
        {
            // Arrange:
            var category = new Category( 10, 21 );
            var list = new List<Category> { category };
            var filter = new CategoryFilter( list );

            // Action:
            var product = new Product() { ItemHeight = 2.0f, ItemLength = 5.0f, ItemWidth = 2.0f };

            // Assert:
            Assert.That( 0 <= filter.GetCategoryIndex( product ) );
        }

        [Test]
        public void GetCategoryIndex__Product_VolumeIsNotContainedInCategoryList__ReturnsNegativeIngex ()
        {
            // Arrange:
            var category = new Category( 10, 20 );
            var list = new List<Category> { category };
            var filter = new CategoryFilter( list );

            // Action:
            var product = new Product() { ItemHeight = 2.0f, ItemLength = 5.0f, ItemWidth = 2.0f };

            // Assert:
            Assert.That( 0 > filter.GetCategoryIndex( product ) );
        }

        #endregion


        #region Factory

        class FakeCategoryFilter : CategoryFilter
        {
            public FakeCategoryFilter ( IEnumerable< Category > categories ) : base( categories ) { }

            public bool ConteinsCalled { get; private set; }
            public bool AddCategoryCalled { get; private set; }
            public bool FillHolesCalled { get; private set; }

            public override bool Contains ( Category category )
            {
                ConteinsCalled = true;

                return base.Contains( category );
            }

            public override void AddCategory ( Category category )
            {
                AddCategoryCalled = true;
                base.AddCategory( category );
            }

            public override void FillHoles ()
            {
                FillHolesCalled = true;
                base.FillHoles();
            }

            public void ResetContainsCalled ()
            {
                ConteinsCalled = false;
            }
        }

        #endregion
    }
}
