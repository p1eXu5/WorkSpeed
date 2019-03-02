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
    public class DataModelExtensionTests
    {
        [ Test ]
        public void GetBreaks_ReturnsExpected ()
        {
            // Arrange:
            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 5 ),
                FirstBreakTime = new TimeSpan( 8, 55, 0),
                Periodicity = TimeSpan.FromHours( 1 )
            };

            var expected = new Queue< Period >( new Period[] {
                new Period( DateTime.Parse( "28.02.2019 13:55:00" ), DateTime.Parse( "28.02.2019 14:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 14:55:00" ), DateTime.Parse( "28.02.2019 15:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 15:55:00" ), DateTime.Parse( "28.02.2019 16:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 16:55:00" ), DateTime.Parse( "28.02.2019 17:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 17:55:00" ), DateTime.Parse( "28.02.2019 18:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 18:55:00" ), DateTime.Parse( "28.02.2019 19:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 19:55:00" ), DateTime.Parse( "28.02.2019 20:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 20:55:00" ), DateTime.Parse( "28.02.2019 21:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 21:55:00" ), DateTime.Parse( "28.02.2019 22:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 22:55:00" ), DateTime.Parse( "28.02.2019 23:00:00" ) ),
                new Period( DateTime.Parse( "28.02.2019 23:55:00" ), DateTime.Parse( "01.03.2019  0:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  0:55:00" ), DateTime.Parse( "01.03.2019  1:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  1:55:00" ), DateTime.Parse( "01.03.2019  2:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  2:55:00" ), DateTime.Parse( "01.03.2019  3:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  3:55:00" ), DateTime.Parse( "01.03.2019  4:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  4:55:00" ), DateTime.Parse( "01.03.2019  5:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  5:55:00" ), DateTime.Parse( "01.03.2019  6:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  6:55:00" ), DateTime.Parse( "01.03.2019  7:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  7:55:00" ), DateTime.Parse( "01.03.2019  8:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  8:55:00" ), DateTime.Parse( "01.03.2019  9:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019  9:55:00" ), DateTime.Parse( "01.03.2019 10:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019 10:55:00" ), DateTime.Parse( "01.03.2019 11:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019 11:55:00" ), DateTime.Parse( "01.03.2019 12:00:00" ) ),
                new Period( DateTime.Parse( "01.03.2019 12:55:00" ), DateTime.Parse( "01.03.2019 13:00:00" ) ),

            } );

            // Action:
            var actual = shortBreaks.GetBreaks( DateTime.Parse( "28.02.2019 13:21:14" ) );

            // Assert:
            Assert.That( actual, Is.EquivalentTo( expected ) );
        }
    }
}
