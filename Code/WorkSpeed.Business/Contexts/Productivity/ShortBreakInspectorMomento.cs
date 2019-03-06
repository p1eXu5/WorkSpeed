using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class ShortBreakInspectorMomento
    {
        private bool _deposit;
        private bool _depositLock;

        public ShortBreakInspectorMomento ()
        {
            ProductivityMap = new Dictionary< Operation, IProductivity >();
            DowntimePeriods = new HashSet< Period >();

            Break = Period.Zero;
            _deposit = false;
            _depositLock = false;
        }

        public Period Break { get; internal set; }
        public Dictionary< Operation, IProductivity > ProductivityMap { get; }
        public HashSet< Period > DowntimePeriods { get; }

        public bool HasDeposit => _deposit;

        public void SetDeposit ()
        {
            if ( !IsDepositLocked() ) {
                _deposit = true;
            }
        }

        public void RemoveDeposit ()
        {
            if ( !IsDepositLocked() ) {
                _deposit = false;
            }
        }

        public void LockDeposit ()
        {
            _depositLock = true;
        }

        public void UnlockDeposit ()
        {
            _depositLock = true;
        }

        public bool IsDepositLocked() => _depositLock;
    }
}
