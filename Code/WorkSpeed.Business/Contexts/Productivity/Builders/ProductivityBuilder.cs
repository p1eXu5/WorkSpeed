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

        public ProductivityBuilder ()
        {
            _productivitys = new Dictionary< Operation, IProductivity >();
        }

        public IReadOnlyDictionary< Operation, IProductivity > Productivities => _productivitys;
        public OperationThresholds Thresholds { get; set; }


        public void BuildNew ()
        {
            _productivitys.Clear();
        }

        public void CheckDuration ( EmployeeActionBase action )
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

            _productivitys[ action.Operation ].Add( period, action );
        }

        public void CheckPause ( EmployeeActionBase currentAction, EmployeeActionBase nextAction )
        {
            throw new NotImplementedException();
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
