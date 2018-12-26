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
        private readonly Dictionary< string, (TimeSpan duration, DayPeriod period) > _variableBreaks;
        private readonly Dictionary< string, List<DayPeriod> > _fixedBreaks;
        private Predicate< Employee > _predicate;

        public BreakRepository ()
        {
            _variableBreaks = new Dictionary< string, (TimeSpan duration, DayPeriod period) >(2);
            _fixedBreaks = new Dictionary<string, List< DayPeriod >>( 2 );
        }

        public TimeSpan GetLongest ( Period period )
        {
            return _variableBreaks.Where( v => period.Duration >= v.Value.duration && v.Value.period.IsIntersects( period.GetDayPeriod ) )
                                  .OrderBy( v => v.Value.period.Start )
                                  .Select( v => v.Value.duration )
                                  .FirstOrDefault();
        }

        public TimeSpan GetShortest ( Employee employee )
        {
            if ( _predicate != null ) {


            }
            return _fixedBreaks.Min( v => v.Value. );
        }

        public TimeSpan CheckFixed ( Period period, Employee employee )
        {
            throw new NotImplementedException();
        }

        public void SetVariableBreak ( string name, TimeSpan duration, DayPeriod period )
        {
            _variableBreaks[ name ] = (duration, period);
        }

        public void SetFixedBreaks ( string name, 
                                     TimeSpan duration, 
                                     TimeSpan interval, 
                                     TimeSpan offset, 
                                     Predicate<Employee> predicate = null )
        {
            if ( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException( "name is null or empty or whitespaces." );

            if ( duration < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "duration must be greater than zero and less than full day", nameof( duration ));
            if ( interval < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "interval must be greater than zero and less than full day", nameof( interval ));
            if ( offset < TimeSpan.Zero && offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException( "interval must be greater than zero and less than full day", nameof( interval ));

            _predicate = predicate;

            _fixedBreaks[ name ] = new List< DayPeriod >();

            TimeSpan start;
            TimeSpan end = offset;
            DayPeriod period;

            do {
                start = end + interval;
                end += interval + duration;
                period = new DayPeriod( start, end );
                _fixedBreaks[ name ].Add( period );

            } while ( end < TimeSpan.FromDays( 1 ) );

            end = offset;

            do {
                start = end - duration - interval;
                period = new DayPeriod( start, end );
                _fixedBreaks[ name ].Add( period );
                end = start;

            } while ( end > TimeSpan.Zero );

        }

        public DayPeriod[] GetFixedBreaks ( string name )
        {
            if ( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException("name is null or empty or whitespaces.");

            if ( _fixedBreaks.ContainsKey( name ) ) {
                return _fixedBreaks[ name ].ToArray();
            }

            return new DayPeriod[0];
        }
    }
}
