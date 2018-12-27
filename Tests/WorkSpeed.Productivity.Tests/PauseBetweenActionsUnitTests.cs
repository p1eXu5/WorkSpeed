using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.Productivity.Tests
{

    [TestFixture]
    public class PauseBetweenActionsUnitTests
    {

        [Test]
        public void Ctor__BreakRepository_IsNull__Throws ()
        {



        }


        [Test]
        public void Ctor__BreakRepository__AddsBreakRepository ()
        {



        }


        [Test]
        public void BreakRepository__ByDefault__NotNull ()
        {




        }


        [Test]
        public void BreakRepository__ByDefault__TypeOfIBankRepository ()
        {




        }



        [ Test ]
        public void MinRestBetweenShifts__ByDefault__ReturnsForHour ()
        {



        }



        [Test]
        public void MinRestBetweenShifts__Setter__AssignValue ()
        {



        }



        [Test]
        public void GetPauseInterval__Action_IsNull__Throws ()
        {



        }



        [Test]
        public void GetPauseInterval__LastAction_IsNull__Throws ()
        {



        }





        [ Test ]
        public void GetPauseInterval__ByDefault__ReturnsTimeBetweenActionAndLastAction ()
        {



        }


        [Test]
        public void GetPauseInterval__ActionTime_IsGreaterLastActionTime_HasNoBreaksInRepo__ReturnsPositiveTime ()
        {



        }

        [Test]
        public void GetPauseInterval__ActionTime_IsLessLastActionTime_HasNoBreaksInRepo__ReturnsPositiveTime ()
        {



        }



        [Test]
        public void GetPauseInterval__ActionTime_IsIntersectLastActionTime_HasNoBreaksInRepo__Throw ()
        {



        }


        [Test]
        public void GetPauseInterval__ByDefault__CallsIBreakRepository ()
        {



        }


        [Test]
        public void GetPauseInterval__ByDefault__PassesNotZeroPeriodToBreakRepository()
        {



        }


        [Test]
        public void ShiftCollection__ByDefault__IsEmpty ()
        {



        }


        [Test]
        public void ShortBreakCollection__ByDefault__IsEmpty ()
        {



        }


    }

}
