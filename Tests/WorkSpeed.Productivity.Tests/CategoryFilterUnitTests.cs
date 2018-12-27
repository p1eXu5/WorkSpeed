using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Productivity.Tests
{
    [ TestFixture ]
    public class CategoryFilterUnitTests
    {





        [Test]
        public void Ctor_CategoriesIsNull_Throws ()
        {







        }




        [Test ]
        public void Ctor_ByDefault_DontCreateCategories ()
        {






        }


        [ Test ]
        public void Ctor__Categories_Empty__DontCreateCategories ()
        {





        }



        [Test]
        public void Ctor__Categories_NotEmpty__AddsCategories ()
        {





        }


        [Test]
        public void Ctor__Category_NegativeMinVolume__Throws ()
        {





        }



        [Test]
        public void Ctor__Category_NegativeMaxVolume__Throws ()
        {





        }




        [Test]
        public void Ctor__Category_MinVolumeLargerMaxVolume__Throws ()
        {





        }


        [Test]
        public void Ctor__Category_MinVolumeEqualsMaxVolume__Throws ()
        {





        }

        #region Properies


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
        public void Category__CategoryList_NotEmpty__ReturnsNotEmptyCategories ()
        {





        }















    }
}
