using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Productivity.Tests
{
    [ TestFixture ]
    public class PauseBetweenActionsIntegrationTests
    {

        [Test]
        public void GetPauseInterval___DiffTime_GreaterThanMinRestBetweenShifts___ClearShiftCollection ()
        {




        }



        [Test]
        public void GetPauseInterval___DiffTime_GreaterThanMinRestBetweenShifts___ClearShortBreakCollection ()
        {




        }



        [Test]
        public void GetPauseInterval___DiffTime_GreaterThanMinRestBetweenShifts___ReturnsZeroTimeSpan ()
        {




        }




        [ Test ]
        public void GetPauseInterval___DiffTime_LessThanMinRestBetweenShifts__Shift_NotInShiftList___ReduceDiffTimeByShiftLunchDuration ()
        {




        }



        [Test]
        public void GetPauseInterval___DiffTime_LessThanMinRestBetweenShifts__Shift_NotInShiftList___AddsShiftIntoShiftList ()
        {




        }



        [Test]
        public void GetPauseInterval___DiffTime_LessThanMinRestBetweenShifts__ShortBreak_NotInShortBreakList___ReduceDiffTimeByShortBreakDuration ()
        {




        }



        [Test]
        public void GetPauseInterval___DiffTime_LessThanMinRestBetweenShifts__ShortBreak_NotInShortBreakList___AddsShortBreakIntoShortBreakList ()
        {




        }



    }
}
