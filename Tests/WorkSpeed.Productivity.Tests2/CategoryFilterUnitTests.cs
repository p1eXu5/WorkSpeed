using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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


        [Test]
        public void Ctor_CategoriesIsNull_Throws ()
        {



        }



        [ Test ]
        public void Ctor__Categories_Empty__Throws ()
        {





        }



        [Test]
        public void Ctor__Categories_NotEmpty__CreatesNotEmptyCategoryList ()
        {





        }


        [Test]
        public void Ctor__Categories_NotEmpty__CallsContains ()
        {





        }



        [Test]
        public void Ctor__Categories_NotEmpty__CallsAddCategory ()
        {





        }



        [Test]
        public void Ctor__Categories_NotEmpty__CallsFillHoles ()
        {





        }


        [Test]
        public void AddCategory__Category_NegativeMinVolume__Throws ()
        {





        }



        [Test]
        public void AddCategory__Category_NegativeMaxVolume__Throws ()
        {





        }




        [Test]
        public void AddCategory__Category_MinVolumeLargerMaxVolume__Throws ()
        {





        }


        [Test]
        public void AddCategory__Category_MinVolumeEqualsMaxVolume__Throws ()
        {





        }


        [Test]
        public void AddCategory__Category_MinVolumeGreaterCategoryListMinVolume__Throws ()
        {





        }


        [Test]
        public void AddCategory__Category_MaxVolumeLessCategoryListMaxVolume__Throws ()
        {





        }




        #region AddCategory


        [Test]
        public void AddCategory__Category_NullName__CreatesCategoryWithNullName ()
        {





        }

        [Test]
        public void AddCategory__Category_NotNullName__CreatesCategoryWithNotNullName ()
        {





        }



        [Test]
        public void AddCategory__Category_Id__CreatesCategoryEqualsId ()
        {





        }

        [Test]
        public void AddCategory__Category_Date__CreatesCategoryEqualsDate ()
        {





        }

        #endregion



        [Test]
        public void CategoryList__ByDefault__ReturnsNotEmptyCategories ()
        {





        }



        [Test]
        public void GetCategoryIndex__Product_IsNull__Throws ()
        {





        }


        [Test]
        public void GetCategoryIndex__Product_VolumeLessZero__Throws ()
        {





        }


        [Test]
        public void GetCategoryIndex__Product_VolumeIsZero__RetunsNegateIndex ()
        {





        }



        [Test]
        public void GetCategoryIndex__Product_VolumeGreaterCategoryMaxVolume__RetunsNegateIndex ()
        {





        }


        [Test]
        public void GetCategoryIndex__Product_VolumeLessCategoryMaxVolume__RetunsNegateIndex ()
        {





        }




        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroMinVolumeCategory ()
        {





        }


        [Test]
        public void FillHoles__CategoryList_WithoutZeroMinVolume__CreatesZeroIdCategory ()
        {





        }


        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesPositiveInfiniteMaxVolumeCategory ()
        {





        }


        [Test]
        public void FillHoles__CategoryList_WithoutPositiveInfiniteMaxVolume__CreatesZeroIdCategory ()
        {





        }




        [Test]
        public void FillHoles__CategoryList_WithVolumeHole__CreatesFillingCategory ()
        {





        }


        [Test]
        public void FillHoles__CategoryList_WithVolumeHole__CreatesZeroIdFillingCategory ()
        {





        }



        [ Test ]
        public void Contains__Category_VolumeInCategoryList__ReturnsTrue ()
        {



        }



        [Test]
        public void Contains__Category_VolumeNotInCategoryList__ReturnsFalse ()
        {



        }



        [Test]
        public void Contains__Category_IsNull__Throws ()
        {



        }

    }
}
