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
        public void Ctor__Shift_IsNull__Throws ()
        {




        }


        [Test]
        public void Ctor__Shift__CallsAddVariableBreak ()
        {




        }


        [Test]
        public void Ctor__ShortBreak_IsNull__Throws ()
        {




        }


        [Test]
        public void Ctor__ShortBreak__CallsAddFixedBreak ()
        {




        }




        [Test]
        public void AddVariableBreak__Shift_IsNull__Throws ()
        {




        }



        [Test]
        public void AddVariableBreak__Shift__AddsVariableBreak ()
        {




        }



        [Test]
        public void AddFixedBreak__ShortBreak_IsNull__Throws ()
        {




        }



        [Test]
        public void AddFixedBreak__ShortBreak__AddsFixedBreak ()
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



        [ Test ]
        public void VariableBreakList__ByDefault__Empty ()
        {



        }


        [Test]
        public void FixedBreakList__ByDefault__Empty ()
        {



        }

    }
}
