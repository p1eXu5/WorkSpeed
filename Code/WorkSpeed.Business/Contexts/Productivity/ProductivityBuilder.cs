﻿using System;
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
        private readonly IShortBreakInspectorFactory _shortBreakInspectorFactory;

        private Dictionary< Operation, IProductivity > _productivityMap;
        private HashSet< Period > _downtimePeriods;

        #region Ctor

        static ProductivityBuilder ()
        {
            _shiftMarker = TimeSpan.FromHours( 5 );
        }

        public ProductivityBuilder ()
        {
            _productivityMap = new Dictionary< Operation, IProductivity >();
            _downtimePeriods = new HashSet< Period >();
            _shortBreakInspectorFactory = new ShortBreakInspectorFactory();
        }

        #endregion


        #region Properties

        /// <summary>
        ///     Returns (productivities, downtimes).
        /// </summary>
        /// <returns></returns>
        public (IReadOnlyDictionary< Operation, IProductivity > productivityMap, HashSet< Period > downtimes) GetResult ()
        {
            return ( _productivityMap, _downtimePeriods );
        }

        public OperationThresholds Thresholds { get; set; }

        #endregion


        public void BuildNew ()
        {
            _productivityMap = new Dictionary< Operation, IProductivity >();
            _downtimePeriods = new HashSet< Period >();
        }

        /// <summary>
        ///     #1. Check operation duration.
        ///     BreakDuration changes for receptions, buyer gatheriond and (probably) for
        ///     packing operation (if it was fast packing).
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public (Period, EmployeeActionBase) CheckDuration ( EmployeeActionBase action )
        {
            if (action.Operation == null) throw new ArgumentException( @"Operation cannot be null.", nameof( action.Operation ) );

            Period period;

            switch ( action.GetOperationGroup() ) {
                case OperationGroups.Packing:
                    var newDuration = (( DoubleAddressAction )action).GetPackingDuration( new[]{ 7.0, 8.0, 9.0, 10.0 } );
                    period = new Period( action.StartTime, action.StartTime.Add( newDuration ) );
                    break;
                case OperationGroups.BuyerGathering:
                    newDuration = (( DoubleAddressAction )action).GetBuyerGatheringDuration( 5, 30, 60 );
                    period = new Period( action.StartTime, action.StartTime.Add( newDuration ) );
                    break;
                case OperationGroups.Reception:
                    period = new Period( action.StartTime, action.StartTime.Add( action.Duration + TimeSpan.FromSeconds( 10 ) ) );
                    break;
                default:
                    period = new Period( action.StartTime, action.StartTime.Add( action.Duration ) );
                    break;
            }

            if ( !_productivityMap.ContainsKey( action.Operation ) ) {
                _productivityMap[ action.Operation ] = new Productivity();
            }
            _productivityMap[ action.Operation ].Add( action, period );
            
            return (period, action);
        }

        /// <summary>
        ///     #2.
        /// </summary>
        /// <param name="currentAction"></param>
        /// <param name="nextAction"></param>
        /// <returns>Tuple from new Perion and current action.</returns>
        public (Period, EmployeeActionBase) CheckPause ( (Period period, EmployeeActionBase action) currentAction, 
                                                         (Period period, EmployeeActionBase action ) nextAction )
        {
            var pause = nextAction.Item1.Start - currentAction.Item1.End;

            if ( pause == TimeSpan.Zero ) { return currentAction; }

            Period newPeriod;

            if ( pause < TimeSpan.Zero ) {

                var currentGroup = currentAction.Item2.GetOperationGroup();
                var nextGroup = nextAction.Item2.GetOperationGroup();

                if ( (currentGroup == OperationGroups.Packing && nextGroup != OperationGroups.Packing)
                     || ( currentGroup == OperationGroups.Packing && nextGroup == OperationGroups.Packing && nextAction.period.Start == nextAction.action.StartTime ) ) {

                    newPeriod = new Period( currentAction.period.Start.Add( pause ), nextAction.period.Start ); 
                    _productivityMap[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
                    return (newPeriod, currentAction.Item2);
                }
                else if ( nextGroup == OperationGroups.Packing
                          && nextAction.period.Start < nextAction.action.StartTime ) {

                    // если период операции смещался => next.End - invariant

                    Period newNextPeriod;
                    
                    if ( currentAction.period.End >= nextAction.period.End ) {

                        Period newCurrentPeriod;

                        if ( currentAction.period.Start < nextAction.period.Start ) {
                            
                            newCurrentPeriod = new Period( currentAction.period.Start, nextAction.period.Start );
                            _productivityMap[ currentAction.action.Operation ][ currentAction.action ] = newCurrentPeriod;
                            return (newCurrentPeriod, currentAction.action);
                        }

                        // считаем длину текущей операции общей

                        newCurrentPeriod = new Period( currentAction.period.Start, currentAction.period.GetMedian() );
                        newNextPeriod = new Period( newCurrentPeriod.End, currentAction.period.End );

                        _productivityMap[ nextAction.action.Operation ][ nextAction.action ] = newNextPeriod;
                        _productivityMap[ currentAction.action.Operation ][ currentAction.action ] = newCurrentPeriod;
                        return (newCurrentPeriod, currentAction.action);
                    }
                    else {

                        newNextPeriod = new Period( currentAction.period.End, nextAction.period.End );
                        _productivityMap[ nextAction.action.Operation ][ nextAction.action ] = newNextPeriod;
                        return currentAction;
                    }
                }
                else {

                    newPeriod = new Period( currentAction.Item1.Start, nextAction.Item1.Start ); 
                    _productivityMap[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
                    return (newPeriod, currentAction.Item2);
                }
            }

            if ( pause.TotalSeconds <= (Thresholds?[ currentAction.Item2.Operation ] ?? 20) ) {

                newPeriod = new Period( currentAction.Item1.Start, currentAction.Item1.End.Add( pause ) );
            }
            else {
                newPeriod = new Period( 
                    start: currentAction.Item1.Start, 
                    end: currentAction.Item1.End.Add( TimeSpan.FromSeconds( (Thresholds?[ currentAction.Item2.Operation ] ?? 20) ) ) 
                );

                _downtimePeriods.Add( new Period( newPeriod.End, nextAction.Item1.Start ) );
            }

            _productivityMap[ currentAction.Item2.Operation ][ currentAction.Item2 ] = newPeriod;
            return (newPeriod, currentAction.Item2);
        }

        /// <summary>
        ///     #3. Substracts shortBreaks from downtime.
        /// </summary>
        /// <param name="shortBreaks"></param>
        public void SubstractBreaks ( ShortBreakSchedule shortBreaks )
        {
            // each employee must finish current action before leaving for a break
            if ( !_downtimePeriods.Any() ) { return; }

            var maxDowntime = shortBreaks.Periodicity + shortBreaks.Periodicity - shortBreaks.Duration;
            var dtArr = _downtimePeriods.Where( d => d.Duration < maxDowntime ).OrderBy( d => d.Start ).ToArray();
            var inspector = _shortBreakInspectorFactory.GetShortBreakInspector( shortBreaks );
            var firstDowntime = dtArr[ 0 ];

            Period brk;
            var momento = inspector.SetBreak( firstDowntime );

            if ( momento.Break.Start.TimeOfDay > inspector.FirstBreakTime ) {
                momento.SetDeposit();
                brk = inspector.GetPreviousBreak( ref momento );
            }
            else {
                brk = momento.Break;
            }

            foreach ( var downtimePeriod in dtArr ) {

                if ( inspector.IsBreak( downtimePeriod, ref momento ) ) {

                    RemoveBreakFromDowntime( downtimePeriod, momento.Break );
                    brk = momento.Break;
                }
                else if ( momento.HasDeposit ) {
                    var operation = _productivityMap.FirstOrDefault( p => p.Key.Group == OperationGroups.Shipment && p.Value.Any( a => a.IsIntersectsWith( brk ) ) ).Key;
                    if ( operation != null ) {

                        var downtimes = _downtimePeriods.Where( p => p > brk && p < momento.Break ).OrderBy( p => p.Duration ).ToArray();

                        if ( downtimes.Length > 0 ) {
                            int i = 0;
                            while ( downtimes[ i ].Duration < brk.Duration || i < downtimes.Length ) { ++i; }
                            var momentoBreak = new Period( downtimes[ i ].Start, downtimes[ i ].Start.Add( brk.Duration ) );
                            RemoveBreakFromDowntime( downtimes[ i ], momentoBreak );
                        }
                    }
                    momento.RemoveDeposit();
                }
            }
        }

        /// <summary>
        ///     #4.
        /// </summary>
        /// <param name="shift"></param>
        public void SubstractLunch ( Shift shift )
        {
            foreach ( var periods in GetShiftPeriods() ) {

                var period = periods.FirstOrDefault( p => p.Duration >= shift.Lunch );
                if ( period == default( Period ) ) { continue; }

                _downtimePeriods.Remove( period );

                var newPeriod = period.CutEnd( shift.Lunch );
                if ( newPeriod.Duration > TimeSpan.Zero ) {
                    _downtimePeriods.Add( newPeriod );
                }
            }
        }


        /// <summary>
        ///     Returns each shift periods.
        /// </summary>
        /// <returns></returns>
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

        private void RemoveBreakFromDowntime ( Period downtimePeriod, Period @break )
        {
            _downtimePeriods.Remove( downtimePeriod );

            var newDowntime = downtimePeriod - @break;
            if ( newDowntime.Duration > TimeSpan.Zero ) {
                var removed = downtimePeriod.Duration - newDowntime.Duration;

                if ( removed == @break.Duration ) {
                    _downtimePeriods.Add( newDowntime );
                }
                else if ( downtimePeriod.Duration > @break.Duration && removed < @break.Duration ) {
                    var diffPeriod = new Period( @break.End, @break.End.Add( @break.Duration - removed ) );
                    newDowntime -= diffPeriod;
                    _downtimePeriods.Add( newDowntime );
                }
            }
        }
    }
}
