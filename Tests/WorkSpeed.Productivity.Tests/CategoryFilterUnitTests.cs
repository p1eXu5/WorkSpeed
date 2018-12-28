using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
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
        public void Ctor__Categories_NotEmpty__CreatesNotEmptyCategoryList ()
        {
            List<Category> list = new List<Category>();
            list.Add( new Category( 0, 10 ) );

            Assert.That( new CategoryFilter( list ).CategoryList.Any() );
        }

        #endregion


        #region ContainsVolume

        [Test]
        public void Contains__Category_IsNull__Throws ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );

            // Action:
            // Assert:
            Assert.Catch< ArgumentNullException >( () => filter.ContainsVolume( null ) );
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
            Assert.Catch<ArgumentException>( () => filter.ContainsVolume( categoryWithMaxVolumeLessMinVolume ) );
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
            Assert.Catch<ArgumentException>( () => filter.ContainsVolume( categoryWithNegativeMinVolume ) );
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
            Assert.Catch<ArgumentException>( () => filter.ContainsVolume( categoryWithNegativeMaxVolume ) );
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
            Assert.Catch<ArgumentException>( () => filter.ContainsVolume( categoryMinVolumeEqualsMaxVolume ) );
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
            Assert.That( true == filter.ContainsVolume( containedCategory ) );
        }

        [TestCase( 0, 9 )]
        [TestCase( 21, 25 )]
        public void Contains__Category_VolumeNotInCategoryList__ReturnsFalse ( int min, int max )
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );
            var containedCategory = new Category( min, max );

            // Action:
            // Assert:
            Assert.That( false == filter.ContainsVolume( containedCategory ) );
        }

        #endregion


        #region FillHoles

        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroMinVolumeCategory ()
        {
            // Arrange:
            var list = new List< Category > { new Category( 10, 20) };
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
            var zeroMinVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MinVolume.Equals( 0.0 ) );

            // Assert:
            Assert.That( zeroMinVolumeCategory, Is.Not.Null );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroIdCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
            var zeroMinVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MinVolume.Equals( 0.0 ) );

            // Assert:
            Assert.That( 0 == zeroMinVolumeCategory?.Id );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesPositiveInfiniteMaxVolumeCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
            var zeroMaxVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MaxVolume.Equals( double.PositiveInfinity ) );

            // Assert:
            Assert.That( zeroMaxVolumeCategory, Is.Not.Null );
        }

        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesZeroIdCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ) };
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
            var zeroMaxVolumeCategory = filter.CategoryList.FirstOrDefault( c => c.MaxVolume.Equals( double.PositiveInfinity ) );

            // Assert:
            Assert.That( 0 == zeroMaxVolumeCategory?.Id );
        }

        [Test]
        public void FillHoles__CategoryList_WithVolumeHole__CreatesFillingCategory ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 10, 20 ), new Category( 30, 40) };
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
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
            var filter = new CategoryFilter( list );

            // Action:
            filter.FillHoles();
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
        public void AddCategory__Category_NullName__CreatesCategoryWithNullName ()
        {
            // Arrange:
            var category = new Category( 10, 20 ) { Name = null };
            var list = new List<Category> { category };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.CategoryList.First().Name, Is.Null );
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
            Assert.That( filter.CategoryList.First().Name, Is.Not.Null );
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
            Assert.That( 5 == filter.CategoryList.First().Id );
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
            Assert.That( filter.CategoryList.First().Date.Equals( date ) );
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


        #region HasHoles

        [Test]
        public void HasHoles__CategoryList_Single_WithoutZeroMinVolume__ReturnsTrue ()
        {
            // Arrange:
            var list = new List<Category> { new Category ( 10.0, double.PositiveInfinity )  };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.HasHoles );
        }

        [Test]
        public void HasHoles__CategoryList_Single_WithoutPositiveInfinityMaxVolume__ReturnsTrue ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 0.0, 10.0 ) };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.HasHoles );
        }

        [Test]
        public void HasHoles__CategoryList_Single_WithZeroAndPositiveInfinityMaxVolume__ReturnsFalse ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 0.0, Double.PositiveInfinity ) };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( !filter.HasHoles() );
        }

        [Test]
        public void HasHoles__CategoryList_Single_WithoutZeroAndPositiveInfinityMaxVolume__ReturnsTrue ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 2.0, 3.9 ) };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.HasHoles );
        }

        [Test]
        public void HasHoles__CategoryList_WithoutZeroAndPositiveInfinityMaxVolume__ReturnsTrue ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 2.0, 3.9 ), new Category(5, 7) };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.HasHoles );
        }

        [Test]
        public void HasHoles__CategoryList_WithHole__ReturnsTrue ()
        {
            // Arrange:
            var list = new List<Category> { new Category( 0.0, 3.9 ), new Category( 5.0, Double.PositiveInfinity ) };

            // Action:
            var filter = new CategoryFilter( list );

            // Assert:
            Assert.That( filter.HasHoles );
        }

        #endregion

    }
}
