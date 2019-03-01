using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.Data.Models.Extensions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class ProductivityBuilder : IProductivityBuilder
    {
        private static readonly TimeSpan _shiftMarker;

        private readonly Dictionary< Operation, IProductivity > _productivitys;
        private readonly HashSet< Period > _downtimePeriods;

        static ProductivityBuilder ()
        {
            _shiftMarker = TimeSpan.FromHours( 5 );
        }

        public ProductivityBuilder ()
        {
            _productivitys = new Dictionary< Operation, IProductivity >();
            _downtimePeriods = new HashSet< Period >();
        }

        /// <summary>
        ///     Returns (productivities, downtimes).
        /// </summary>
        /// <returns></returns>
        public (IReadOnlyDictionary< Operation, IProductivity >, HashSet< Period >) GetResult ()
        {
            return ( _productivitys, _downtimePeriods );
        }

        public OperationThresholds Thresholds { get; set; }


        public void BuildNew ()
        {
            _productivitys.Clear();
            _downtimePeriods.Clear();
        }

        public (Period, EmployeeActionBase) CheckDuration ( EmployeeActionBase action )
        {
            if (action.Operation == null) throw new ArgumentException( @"Operation cannot be null.", nameof( action.Operation ) );

            Period period;

            switch ( action.GetOperationGroup() ) {
                case OperationGroups.Packing:
                    period = new Period( action.StartTime, action.StartTime.Add( (( DoubleAddressAction )action).GetPlacingDuration( new[]{ 7.0, 8.0, 9.0, 10.0 } ) ) );
                    break;
                case OperationGroups.BuyerGathering:
                    period = new Period( action.StartTime, action.StartTime.Add((( DoubleAddressAction )action).GetBuyerGatheringDuration( 5, 30, 60 ) ) );
                    break;
                case OperationGroups.Reception:
                    period = new Period( action.StartTime, action.StartTime.Add( action.Duration + TimeSpan.FromSeconds( 10 ) ) );
                    break;
                default:
                    period = new Period( action.StartTime, action.StartTime.Add( action.Duration ) );
                    break;
            }

            if ( !_productivitys.ContainsKey( action.Operation ) ) {
                _productivitys[ action.Operation ] = new Productivity();
            }
            _productivitys[ action.Operation ].Add( action, period );
            
            return (period, action);
        }

        public (Period, EmployeeActionBase) CheckPause ( (Period, EmployeeActionBase) currentAction, (Period, EmployeeActionBase) nextAction )
        {
            var pause = nextAction.Item1.Start - currentAction.Item1.End;

            if ( pause == TimeSpan.Zero ) { return currentAction; }

            Period newPeriod;

            if ( pause < TimeSpan.Zero ) {

                var currentGroup = currentAction.Item2.GetOperationGroup();
                var nextGroup = nextAction.Item2.GetOperationGroup();

                if ( currentGroup == OperationGroups.Packing ) {

                    newPeriod = new Period( currentAction.Item1.Start.Add( pause ), nextAction.Item1.Start ); 
                    _productivitys[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
                    return (newPeriod, currentAction.Item2);
                }
                else if ( nextGroup == OperationGroups.Packing ) {

                    newPeriod = new Period( currentAction.Item1.End, nextAction.Item1.End.Subtract( pause ) );
                    _productivitys[ nextAction.Item2.Operation ][ nextAction.Item2 ] = newPeriod;
                    return currentAction;
                }
                else {

                    newPeriod = new Period( currentAction.Item1.Start, nextAction.Item1.Start ); 
                    _productivitys[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
                    return (newPeriod, currentAction.Item2);
                }
            }

            if ( pause.Seconds <= Thresholds[ currentAction.Item2.Operation ] ) {

                newPeriod = new Period( currentAction.Item1.Start, currentAction.Item1.End.Add( pause ) );
            }
            else {
                newPeriod = new Period( currentAction.Item1.Start, currentAction.Item1.End.Add( TimeSpan.FromSeconds( Thresholds[ currentAction.Item2.Operation ] ) ) );
                // save pause in downtime periods
                _downtimePeriods.Add( new Period( newPeriod.End, nextAction.Item1.Start ) );
            }

            _productivitys[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
            return (newPeriod, currentAction.Item2);
        }

        /// <summary>
        /// Substracts breaks from downtime.
        /// </summary>
        /// <param name="breaks"></param>
        public void SubstractBreaks ( ShortBreakSchedule breaks )
        {
            var downtimePeriods = _downtimePeriods.ToArray();

            // each employee must finish current action before leaving for a break
            if ( downtimePeriods.Length == 0 ) { return; }

            Queue< Period > breakQueue = breaks.GetBreaks( downtimePeriods.First().Start );
            Period brk;
            ChangeBreak();

            bool debt = false;

            foreach ( var downtimePeriod in _downtimePeriods.ToArray() ) {

                if ( downtimePeriod.Contains( brk ) ) {

                    _downtimePeriods.Remove( downtimePeriod );
                    _downtimePeriods.Add( downtimePeriod - brk );

                    if ( debt ) {
                        // it's mean employee lost the break
                        debt = false;
                    }
                    continue;
                }
                
                if ( debt ) {
                    if ( downtimePeriod.Contains( breaks.Duration ) ) {

                        _downtimePeriods.Remove( downtimePeriod );
                        _downtimePeriods.Add( downtimePeriod.CutEnd( breaks.Duration ) );
                        debt = false;
                    }
                }

                if ( downtimePeriod < brk ) {

                    // not large and not intersects
                    continue;
                }

                debt = CheckActions();
                ChangeBreak();
            }

            bool CheckActions()
            {
                return _productivitys.Values.FirstOrDefault( p => p.FirstOrDefault( per => per.Contains( brk )  ) != default( Period ) ) != null;
            }

            void ChangeBreak ()
            {
                brk = breakQueue.Dequeue();
                breakQueue.Enqueue( brk );
            }
        }


        public void SubstractLunch ( Shift shift )
        {
            foreach ( var periods in GetShiftPeriods() ) {

                var period = periods.FirstOrDefault( p => p.Contains( shift.Lunch ) );
                if ( period != default( Period ) ) { continue; }

                _downtimePeriods.Remove( period );
                _downtimePeriods.Add( period.CutEnd( shift.Lunch ) );
            }
        }

        private IEnumerable< IEnumerable< Period >> GetShiftPeriods ()
        {
            Period period;
            var count = 0;

            do {
                period = _downtimePeriods.Skip( count ).FirstOrDefault( p => p.Duration > _shiftMarker );

                if ( period == default( Period ) ) {

                    yield return _downtimePeriods.Skip( count );
                }
                else {
                    var res = _downtimePeriods.Skip( count ).Where( p => p < period ).ToArray();
                    count = res.Length;
                    yield return res;
                }

            } while ( period != default( Period ) );
        }
    }
}
