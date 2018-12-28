using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Productivity.Tests
{
    [ TestFixture]
    public class BreakRepositoryUnitTests
    {
        [ Test ]
        public void ShiftCollection__ByDefault__IsEmpty ()
        {
            var repo = new BreakRepository();

            Assert.That( !repo.ShiftCollection.Any() );
        }

        [Test]
        public void ShortBreakCollection__ByDefault__IsEmpty ()
        {
            var repo = new BreakRepository();

            Assert.That( !repo.ShortBreakCollection.Any() );
        }


        [Test]
        public void AddVariableBreak__Shift_IsNull__Throw ()
        {




        }

        [ Test ]
        public void AddVariableBreak__Shift_DurationIsNotPositive__Throw ()
        {




        }



        [Test]
        public void AddFixedBreak__ShortBreak_IsNull__Throws ()
        {




        }



        [Test]
        public void AddFixedBreak__ShortBreak_IsNotPositive__Throws ()
        {




        }



        [ Test ]
        public void CheckForABreak__Period_EqualsFirstVariableBreakDuration__ReturnsShift ()
        {




        }



        [Test]
        public void CheckForABreak__Period_EqualsFirstShortBreakDuration__ReturnsShortBreak ()
        {




        }

    }
}
