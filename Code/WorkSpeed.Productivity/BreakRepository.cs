using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class BreakRepository : IBreakRepository
    {
        private readonly Dictionary< TimeSpan, ( string name, DayPeriod period) > _variableBreaks;
        private readonly Dictionary< Predicate<Employee>, (string name, List< DayPeriod > breakList) > _fixedBreaks;

        public BreakRepository ()
        {
            _variableBreaks = new Dictionary< TimeSpan, (string, DayPeriod period) >( 2 );
            _fixedBreaks = new Dictionary< Predicate<Employee>, (string name, List<DayPeriod> period) >( 2 );
        }

        public TimeSpan GetLongest ( Period period )
        {
            return _variableBreaks.Where( v => period.Duration >= v.Key && v.Value.period.IsIntersects( period.GetDayPeriod() ) )
                                  .OrderBy( v => v.Key )
                                  .Select( v => v.Key )
                                  .FirstOrDefault();
        }

        public TimeSpan GetShortest ( Employee employee )
        {
            var fixedBreaks = _fixedBreaks.FirstOrDefault( f => f.Key( employee ) );

            if ( fixedBreaks.Key ==  null ) return TimeSpan.Zero;

            return fixedBreaks.Value.breakList[ 0 ].Duration;
        }

        public TimeSpan CheckFixed ( Period period, Employee employee )
        {
            var fixedBreaks = _fixedBreaks.FirstOrDefault( f => f.Key( employee ) );

            if ( fixedBreaks.Key == null ) return period.Duration;

            var duration = period.Duration;
            var days = period.GetDays();

            foreach ( var theBreak in fixedBreaks.Value.breakList ) {

                if ( days.Length > 1 ) {

                    foreach ( var day in days ) {

                        if ( period.Contains( theBreak.GetDatePeriod( day ) ) ) {
                            duration -= theBreak.Duration;
                        }
                    }
                }
                else if ( period.Contains( theBreak.GetDatePeriod( period.Start ) ) ) {
                    duration -= theBreak.Duration;
                }
            }

            return duration;
        }

        public void SetVariableBreak ( string name, TimeSpan duration, DayPeriod period )
        {
            if ( duration < TimeSpan.Zero ) throw new ArgumentException();

            _variableBreaks[ duration ] = (name, period);
        }

        public void SetFixedBreaks ( string name, 
                                     TimeSpan duration, 
                                     TimeSpan interval, 
                                     TimeSpan offset, 
                                     Predicate<Employee> predicate )
        {
            if ( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException( "name is null or empty or whitespaces." );

            if ( duration < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "duration must be greater than zero and less than full day", nameof( duration ));
            if ( interval < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "interval must be greater than zero and less than full day", nameof( interval ));
            if ( offset < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "interval must be greater than zero and less than full day", nameof( interval ));

            if ( predicate == null ) throw new ArgumentNullException(nameof( predicate ), "predicate cannot be null");

            var periodList = new List< DayPeriod >();

            TimeSpan start;
            TimeSpan end = offset;
            DayPeriod period;

            do {
                start = end + interval;
                end += interval + duration;
                period = new DayPeriod( start, end );
                periodList.Add( period );

            } while ( end < TimeSpan.FromDays( 1 ) );

            var lastEnd = end > TimeSpan.FromDays( 1 ) ? end - TimeSpan.FromDays( 1 ) : TimeSpan.Zero ;

            end = offset;
            start = end - duration;

            while ( start > lastEnd ) {

                period = new DayPeriod( start, end );
                periodList.Add( period );
                end = start - interval;
                start = end - duration;
            }

            _fixedBreaks[ predicate ] = (name, periodList);
        }

        public DayPeriod[] GetFixedBreaks ( string name )
        {
            if ( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException("name is null or empty or whitespaces.");

            return _fixedBreaks.Where( fb => fb.Value.name.Equals( name ) ).Select( fb => fb.Value.breakList ).First().ToArray();
        }
    }
}
