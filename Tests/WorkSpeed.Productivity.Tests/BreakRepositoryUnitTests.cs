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

            var shortBreak = new ShortBreakSchedule()
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

            var shortBreak = new ShortBreakSchedule()
            {
                Duration = repo.ShortBreakUpLimit + TimeSpan.FromSeconds( 1 ),
                Periodicity = repo.ShortBreakIntervalUpLimit,
            };

            Assert.Catch<ArgumentException>( () => repo.GetDayPeriods( shortBreak ) );
        }

        [ Test ]
        public void AddVariableBreak__Shift_Valid__AddsShift ()
        {
            // Arrange:
            var shift = GetDayShift();
            var breakRepo = GetBreakRepository();

            // Action:
            breakRepo.AddVariableBreak( shift );

            // Assert:
            Assert.That( shift == breakRepo.ShiftCollection.First() );
        }



        [Test]
        public void AddFixedBreak__ShortBreak_Valid__AddsShortBreak ()
        {
            // Arrange:
            var shortBreak = GetShortBreakForNotSmokers();
            var breakRepo = GetBreakRepository();

            // Action:
            breakRepo.AddFixedBreak( shortBreak, (e) => !e.IsSmoker );

            // Assert:
            Assert.That( shortBreak == breakRepo.ShortBreakCollection.First() );
        }



        [Test]
        public void GetDayPeriods__ShortBreak_ForNotSmokeres__ReturnsExpectedDayPeriodCollection ()
        {
            // Arrange:
            var shortBreak = GetShortBreakForNotSmokers();

            var expectedColl = new DayPeriod[] {

                new DayPeriod( new TimeSpan( 1, 55, 0 ), new TimeSpan( 2, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 3, 55, 0 ), new TimeSpan( 4, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 5, 55, 0 ), new TimeSpan( 6, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 7, 55, 0 ), new TimeSpan( 8, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 9, 55, 0 ), new TimeSpan( 10, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 11, 55, 0 ), new TimeSpan( 12, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 13, 55, 0 ), new TimeSpan( 14, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 15, 55, 0 ), new TimeSpan( 16, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 17, 55, 0 ), new TimeSpan( 18, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 19, 55, 0 ), new TimeSpan( 20, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 21, 55, 0 ), new TimeSpan( 22, 5, 0 ) ),
                new DayPeriod( new TimeSpan( 23, 55, 0 ), new TimeSpan( 0, 5, 0 ) ),
            };

            var breakRepo = GetBreakRepository();

            // Action:
            var res = breakRepo.GetDayPeriods( shortBreak );

            // Assert:
            Assert.That( res, Is.EquivalentTo( expectedColl ) );
        }



        [ Test ]
        public void CheckShortBreak__Period_InBreakInterval_Employee_Valid__ReturnsShortBreak ()
        {
            // Arrange:
            var shortBreak = GetShortBreakForNotSmokers();
            var employee = GetEmployeeNotSmoker();
            var period = new Period( new DateTime(2018, 12, 12, 9, 54, 0), new DateTime( 2018, 12, 12, 10, 6, 0 ) );
            var breakRepo = GetBreakRepository();
            breakRepo.AddFixedBreak( shortBreak, (e) => !e.IsSmoker );

            // Action:
            var res = breakRepo.CheckShortBreak( period, employee );

            // Assert:
            Assert.That( shortBreak == res.shortBreak );
        }

        [Test]
        public void CheckShortBreak__Period_OutOfBreakInterval_Employee_Valid__ReturnsNullShortBreak ()
        {
            // Arrange:
            var shortBreak = GetShortBreakForNotSmokers();
            var employee = GetEmployeeNotSmoker();
            var period = new Period( new DateTime( 2018, 12, 12, 9, 35, 0 ), new DateTime( 2018, 12, 12, 9, 46, 0 ) );
            var breakRepo = GetBreakRepository();
            breakRepo.AddFixedBreak( shortBreak, ( e ) => !e.IsSmoker );

            // Action:
            var res = breakRepo.CheckShortBreak( period, employee );

            // Assert:
            Assert.That( null == res.shortBreak );
        }



        [Test]
        public void CheckLongBreak__Period_InBreakPeriod__ReturnsShift ()
        {
            // Arrange:
            var shift = GetDayShift();
            var breakRepo = GetBreakRepository();
            breakRepo.AddVariableBreak( shift );
            var period = new Period( new DateTime( 2018, 12, 12, 14, 15, 0 ), new DateTime( 2018, 12, 12, 14, 46, 0 ) );

            // Action:
            var res = breakRepo.CheckLunchBreak( period );

            // Assert:
            Assert.That( shift == res[ 0 ] );

        }

        [Test]
        public void CheckLongBreak__Period_OutOfBreakPeriod__ReturnsNull ()
        {
            // Arrange:
            var shift = GetDayShift();
            var breakRepo = GetBreakRepository();
            breakRepo.AddVariableBreak( shift );
            var period = new Period( new DateTime( 2018, 12, 12, 22, 15, 0 ), new DateTime( 2018, 12, 12, 23, 0, 0 ) );

            // Action:
            var res = breakRepo.CheckLunchBreak( period );

            // Assert:
            Assert.That( !res.Any() );
        }


        #region Factory

        private Employee GetEmployeeNotSmoker ()
        {
            return new Employee {
                Id = "AR12345",
                Name = "Test Employee",
                IsSmoker = false
            };
        }

        private BreakRepository GetBreakRepository ()
        {
            return new BreakRepository();
        }

        private Shift GetDayShift ()
        {
            var shift = new Shift() {
                Id = 1,
                Name = "Day Shift",
                StartTime = new TimeSpan( 8, 0, 0 ),
                ShiftDuration = TimeSpan.FromHours( 12 ),
                LunchDuration = TimeSpan.FromMinutes( 30 )
            };

            return shift;
        }

        private ShortBreakSchedule GetShortBreakForNotSmokers ()
        {
            var shortBreak = new ShortBreakSchedule
            {
                Id = 1,
                Periodicity = TimeSpan.FromHours( 2 ),
                Duration = TimeSpan.FromMinutes( 10 ),
                DayOffsetTime = new TimeSpan( 8, 5, 0 )
            };

            return shortBreak;
        }

        private ShortBreakSchedule GetShortBreakForSmokers ()
        {
            var shortBreak = new ShortBreakSchedule {

                Id = 2,
                Periodicity = TimeSpan.FromHours( 1 ),
                Duration = TimeSpan.FromMinutes( 5 ),
                DayOffsetTime = new TimeSpan( 8, 0, 0)
            };

            return shortBreak;
        }

        #endregion
    }
}
