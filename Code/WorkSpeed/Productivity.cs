using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed
{
    public class Productivity
    {
        private readonly TimeSpan THRESHOLD = TimeSpan.FromMinutes( 2 );

        private List< Period > _smokeBreaks;
        private List< Period > _unsmokeBreaks;

        private GatheringAction _lastGatheringAction;

        private Period _gatheringPeriod;
        private TimeSpan _gatheringDuration;

        private TimeSpan _offTime;

        private int _gatheringLines;
        private int _gatheringQuantity;

        public Productivity (Employee employee)
        {
            Employee = employee ?? throw new ArgumentNullException();

            SetSmokeBreakes();
            SetUnsmokeBreakes();
            SetThresholds();
        }

        private void SetSmokeBreakes ()
        {

        }

        private void SetUnsmokeBreakes ()
        {

        }

        private void SetThresholds ()
        {

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
            if ( !gatheringAction.IsGatheringOperation() && !gatheringAction.IsPackingOperation() ) return;

            _gatheringPeriod.Start = gatheringAction.StartTime;
            _gatheringPeriod.End = _gatheringPeriod.Start + gatheringAction.Duration;
            _gatheringDuration = gatheringAction.Duration;

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
