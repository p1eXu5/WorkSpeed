using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Data.Models;

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
        public void GetDayPeriods__ShortBreak_IsNull__Throw ()
        {
            var repo = new BreakRepository();
            Assert.Catch< ArgumentNullException >( () => repo.GetDayPeriods( null ) );
        }

        [Test]
        public void GetDayPeriods__ShortBreak_DurationLessShortBreakDownLimit__Throw ()
        {
            var repo = new BreakRepository();

            var shortBreak = new ShortBreak()
            {
                Duration = repo.ShortBreakDownLimit - TimeSpan.FromSeconds( 1 ),
                Periodicity = repo.ShortBreakIntervalUpLimit,
            };

            Assert.Catch<ArgumentException>( () => new BreakRepository().GetDayPeriods( shortBreak ) );
        }

        [Test]
        public void GetDayPeriods__ShortBreak_DurationGreaterShortBreakUpLimit__Throw ()
        {
            var repo = new BreakRepository();

            var shortBreak = new ShortBreak()
            {
                Duration = repo.ShortBreakUpLimit + TimeSpan.FromSeconds( 1 ),
                Periodicity = repo.ShortBreakIntervalUpLimit,
            };

            Assert.Catch<ArgumentException>( () => repo.GetDayPeriods( shortBreak ) );
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
