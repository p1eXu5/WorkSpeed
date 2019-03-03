using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Business.Contexts.Productivity;

namespace WorkSpeed.Business.Tests.Contexts.Productivity.UnitTests
{
    [ TestFixture ]
    public class PeriodTests
    {
        [ Category( "IsIntersactsWith" ) ]
        [ TestCase( "1.01.2020 1:00:00", "1.01.2020 2:00:00" ) ]
        [ TestCase( "1.01.2020 0:00:00", "1.01.2020 3:00:00" ) ]
        [ TestCase( "1.01.2020 1:10:00", "1.01.2020 1:20:00" ) ]
        [ TestCase( "1.01.2020 0:00:00", "1.01.2020 1:20:00" ) ]
        [ TestCase( "1.01.2020 1:10:00", "1.01.2020 3:00:00" ) ]
        public void IsIntersactsWith_IntersectedPeriods_ReturnsTrue ( string periodAStart, string periodAEnd )
        {
            // Arrange:
            var periodA = new Period( DateTime.Parse( periodAStart ), DateTime.Parse( periodAEnd ) );
            var periodB = new Period( DateTime.Parse( "1.01.2020 1:00:00" ), DateTime.Parse( "1.01.2020 2:00:00" ) );

            // Action:
            // Assert:
            Assert.That( periodA.IsIntersectsWith( periodB ) );

        }

        [ Category( "IsIntersactsWith" ) ]
        [ TestCase( "1.01.2020 0:00:00", "1.01.2020 1:00:00" ) ]
        [ TestCase( "1.01.2020 2:00:00", "1.01.2020 3:00:00" ) ]
        public void IsIntersactsWith_NotIntersectedPeriods_ReturnsFalse ( string periodAStart, string periodAEnd )
        {
            // Arrange:
            var periodA = new Period( DateTime.Parse( periodAStart ), DateTime.Parse( periodAEnd ) );
            var periodB = new Period( DateTime.Parse( "1.01.2020 1:00:00" ), DateTime.Parse( "1.01.2020 2:00:00" ) );

            // Action:
            // Assert:
            Assert.That( !periodA.IsIntersectsWith( periodB ) );
        }

    }
}
