using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Tests.Contexts.Productivity.UnitTests
{
    [ TestFixture ]
    public class ShortBreakInspectorTests
    {
        [ Test ]
        public void BreaksGetter__ReturnsExpected ()
        {
            // Arrange:
            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var expected = new List< (TimeSpan, TimeSpan) >( new (TimeSpan, TimeSpan)[] {
                (TimeSpan.Parse( "0:55:00" ), TimeSpan.Parse( "1:00:00" ) ),
                (TimeSpan.Parse( "1:55:00" ), TimeSpan.Parse( "2:00:00" ) ),
                (TimeSpan.Parse( "2:55:00" ), TimeSpan.Parse( "3:00:00" ) ),
                (TimeSpan.Parse( "3:55:00" ), TimeSpan.Parse( "4:00:00" ) ),
                (TimeSpan.Parse( "4:55:00" ), TimeSpan.Parse( "5:00:00" ) ),
                (TimeSpan.Parse( "5:55:00" ), TimeSpan.Parse( "6:00:00" ) ),
                (TimeSpan.Parse( "6:55:00" ), TimeSpan.Parse( "7:00:00" ) ),
                (TimeSpan.Parse( "7:55:00" ), TimeSpan.Parse( "8:00:00" ) ),
                (TimeSpan.Parse( "8:55:00" ), TimeSpan.Parse( "9:00:00" ) ),
                (TimeSpan.Parse( "9:55:00" ), TimeSpan.Parse( "10:00:00" ) ),
                (TimeSpan.Parse( "10:55:00" ), TimeSpan.Parse( "11:00:00" ) ),
                (TimeSpan.Parse( "11:55:00" ), TimeSpan.Parse( "12:00:00" ) ),
                (TimeSpan.Parse( "12:55:00" ), TimeSpan.Parse( "13:00:00" ) ),
                (TimeSpan.Parse( "13:55:00" ), TimeSpan.Parse( "14:00:00" )),
                (TimeSpan.Parse( "14:55:00" ), TimeSpan.Parse( "15:00:00" ) ),
                (TimeSpan.Parse( "15:55:00" ), TimeSpan.Parse( "16:00:00" ) ),
                (TimeSpan.Parse( "16:55:00" ), TimeSpan.Parse( "17:00:00" ) ),
                (TimeSpan.Parse( "17:55:00" ), TimeSpan.Parse( "18:00:00" ) ),
                (TimeSpan.Parse( "18:55:00" ), TimeSpan.Parse( "19:00:00" ) ),
                (TimeSpan.Parse( "19:55:00" ), TimeSpan.Parse( "20:00:00" ) ),
                (TimeSpan.Parse( "20:55:00" ), TimeSpan.Parse( "21:00:00" ) ),
                (TimeSpan.Parse( "21:55:00" ), TimeSpan.Parse( "22:00:00" ) ),
                (TimeSpan.Parse( "22:55:00" ), TimeSpan.Parse( "23:00:00" ) ),
                (TimeSpan.Parse( "23:55:00" ), TimeSpan.Parse( "0:00:00" ) ),

            } );

            // Action:
            var inspector = new ShortBreakInspector( shortBreaks );
            // Assert:
            Assert.That( inspector.Breaks, Is.EquivalentTo( expected ) );
        }

        [ TestCase( "2.03.2019 13:53", "2.03.2019 14:01", "2.03.2019 14:00" ) ]
        [ TestCase( "2.03.2019 23:56", "3.03.2019 0:01", "3.03.2019 0:00" ) ]
        [ TestCase( "2.03.2019 20:12", "3.03.2019 20:21", "3.03.2019 20:00" ) ]
        [ TestCase( "2.03.2019 8:55:20", "2.03.2019 9:00", "2.03.2019 9:00" ) ]
        [ TestCase( "2.03.2019 23:55:20", "2.03.2019 23:59:45", "3.03.2019 0:00" ) ]
        [ TestCase( "2.03.2019 23:55:20", "3.03.2019 0:05:45", "3.03.2019 0:00" ) ]
        public void SetBreak_ByDefault_ReturnsMomentoWithExpectedBreakEnd ( string start, string end, string expectedEnd )
        {
             // Arrange:
            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            // Action:
            var inspector = new ShortBreakInspector( shortBreaks );
            var momento = inspector.SetBreak( new Period( DateTime.Parse( start ), DateTime.Parse( end ) ) );

            // Assert:
            Assert.That( momento.Break.End, Is.EqualTo( DateTime.Parse( expectedEnd ) ) );
        }
    }
}
