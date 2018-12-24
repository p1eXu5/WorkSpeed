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
        private readonly TimeSpan THRESHOLD_MIN = TimeSpan.FromMinutes( 2 );
        private readonly TimeSpan THRESHOLD_MAX = TimeSpan.FromMinutes( 10 );

        private List< Period > _smokeBreaks;
        private List< Period > _unsmokeBreaks;

        private GatheringAction _lastGatheringAction;

        private ActionTime _gatheringActionTime;

        private TimeSpan _offTime;

        private int _gatheringLines;
        private int _gatheringQuantity;

        public Productivity (Employee employee)
        {
            Employee = employee ?? throw new ArgumentNullException();

            SetSmokeBreakes();
            SetUnsmokeBreakes();
        }

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

        public Employee Employee { get; }

        public void AddGatheringAction ( GatheringAction gatheringAction )
        {
            if ( _lastGatheringAction == null ) {

                _lastGatheringAction = gatheringAction;
                InitTimeAndCounters( gatheringAction );
                return;
            }

            if ( gatheringAction.IsGatheringOperation() ) {

                    if ( _lastGatheringAction.IsGatheringOperation() ) {


                    }
            }

        }

        private void InitTimeAndCounters ( GatheringAction gatheringAction )
        {
            if ( !gatheringAction.IsGatheringOperation() ) return;

            _gatheringActionTime.Start = gatheringAction.StartTime;
            _gatheringActionTime.End = gatheringAction.StartTime + gatheringAction.Duration;
            _gatheringActionTime.Duration = gatheringAction.Duration;

            ++_gatheringLines;
            _gatheringQuantity = gatheringAction.ProductQuantity;
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
