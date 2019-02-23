using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.Data.Models.Extensions;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public class ProductivityBuilder : IProductivityBuilder
    {
        private readonly Dictionary< Operation, IProductivity > _productivitys;
        private readonly HashSet< Period > _downtimePeriods;

        public ProductivityBuilder ()
        {
            _productivitys = new Dictionary< Operation, IProductivity >();
            _downtimePeriods = new HashSet< Period >();
        }

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

        public void SubstractBreaks ( ShortBreakSchedule breaks )
        {
            throw new NotImplementedException();
        }

        public void SubstractLunch ( Shift shift )
        {
            throw new NotImplementedException();
        }
    }
}
