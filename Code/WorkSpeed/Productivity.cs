using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;

namespace WorkSpeed
{
    public class Productivity
    {
        #region Fields 

        private readonly TimeSpan THRESHOLD_MIN = TimeSpan.FromMinutes( 2 );
        private readonly TimeSpan THRESHOLD_MAX = TimeSpan.FromMinutes( 10 );
        private readonly TimeSpan SMALL_BREAK = TimeSpan.FromMinutes( 5 );
        private readonly TimeSpan BIG_LUNCH_TIME = TimeSpan.FromMinutes( 30 );

        private List< Period > _smokeBreaks;
        private List< Period > _unsmokeBreaks;

        private EmployeeAction _lastAction;

        private Period _bigLunch;

        private Dictionary< int, ActionData > _actionDataDictionary;

        private TimeSpan _offTime;

        private int _gatheringLines;
        private int _gatheringQuantity;

        #endregion


        #region Ctor

        public Productivity (Employee employee)
        {
            Employee = employee ?? throw new ArgumentNullException();

            _actionDataDictionary = new Dictionary< int, ActionData >();

            SetSmokeBreakes();
            SetUnsmokeBreakes();
        }

        #endregion


        public Employee Employee { get; }

        public TimeSpan GatheringTime => _actionDataDictionary[ ( int )OperationGroups.Gathering ].Duration;

        private void SetSmokeBreakes ()
        {
            _smokeBreaks = new List< Period >();

            for ( int i = 0; i < 24; i++ ) {  
                _smokeBreaks.Add( new Period( new TimeSpan( i, 55, 0 ), new TimeSpan( i, 59, 59 ) ) );
            }
        }

        private void SetUnsmokeBreakes ()
        {
            _unsmokeBreaks = new List<Period>();

            for ( int i = 0; i < 24; i += 2 )
            {
                _smokeBreaks.Add( new Period( new TimeSpan( i, 0, 0 ), new TimeSpan( i, 04, 59 ) ) );
                _smokeBreaks.Add( new Period( new TimeSpan( i + 1, 55, 0 ), new TimeSpan( i + 1, 59, 59 ) ) );
            }
        }


        public void AddEmployeeAction ( EmployeeAction employeeAction )
        {
            if ( !employeeAction.Employee.Id.Equals(Employee.Id) ) throw new ArgumentException("Another employee.");
            if ( _lastAction != null && _lastAction.StartTime > employeeAction.StartTime )
                throw new ArgumentException("Actions must be sorted.");

            var pause = GetPause( employeeAction.StartTime );
            pause = TryModifyPause( pause, Employee.IsSmoker );

            AddActionData( employeeAction, pause, _actionDataDictionary[ (int) employeeAction.OperationGroup() ] );

            _lastAction = employeeAction;
        }

        private Period GetPause ( DateTime actionStartTime )
        {
            if ( _lastAction == null ) return new Period();

            return new Period( _lastAction.StartTime.AddSeconds( _lastAction.Duration.TotalSeconds ), actionStartTime );
        }


        private Period TryModifyPause ( Period pause, bool isSmoker )
        {
            if ( pause.Duration < SMALL_BREAK ) {
                return pause;
            }

            if ( pause.Duration >= BIG_LUNCH_TIME && !_bigLunch.IsTheSameDate( pause ) ) {

                _bigLunch = pause;
                return Period.Zero;
            }

            if ( _lastAction.OperationGroup() == OperationGroups.Shipment ) return Period.Zero;

            var breaks = isSmoker ? _smokeBreaks : _unsmokeBreaks;

            foreach ( var breakElem in breaks ) {

                if ( breakElem < pause) continue;
                if ( breakElem > pause) break;
                pause -= breakElem;
            }

            return pause;
        }

        private void AddActionData ( EmployeeAction employeeAction, Period pause, ActionData actionData )
        {
            if ( employeeAction.IsShipmentOperation() ) {
                AddShippingDetails( employeeAction as ShipmentAction, actionData );
            }

            if ( employeeAction.IsGatheringOperation() ) {

                AddGatheringDetails( employeeAction as WithProductAction, pause, actionData );
            }
        }

        private void AddShippingDetails ( ShipmentAction shipment, ActionData actionData )
        {
            throw new NotImplementedException();
        }

        private void AddGatheringDetails ( WithProductAction withProductAction, Period pause, ActionData actionData )
        {
            if ( _lastAction == null ) {

                actionData.Duration = withProductAction.Duration;
                actionData.Start = withProductAction.StartTime;
                actionData.End = withProductAction.StartTime.Add( withProductAction.Duration );
            }
            else if ( _lastAction.OperationGroup() == withProductAction.OperationGroup() ) {

                // Другой следующий документ:
                if ( _lastAction.StartTime != withProductAction.StartTime ) {

                }
            }
            // Предыдущая операция другая:
            else {
                
            }

            ++actionData.Lines;
            actionData.Quantity += withProductAction.ProductQuantity;
            actionData.Weight += withProductAction.Product.Weight * withProductAction.ProductQuantity;
            actionData.Volume += withProductAction.Product.GetVolume() * withProductAction.ProductQuantity;
        }
        

        public TimeSpan GetWorkTime ()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetOffTime ()
        {
            throw new NotImplementedException();
        }

        public int GetGatheredLines ()
        {
            throw new NotImplementedException();
        }

        public double GetGatheringSpeed ()
        {
            throw new NotImplementedException();
        }

    }
}
