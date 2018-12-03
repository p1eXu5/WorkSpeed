using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed;

namespace WorkSpeed.Manager.Models
{
    public struct WorkingTime
    {
        private readonly TimeSpan[] _times;

        public TimeSpan GatheringTime => _times[0];
        public TimeSpan ClientGatheringTime => _times[1];
        public TimeSpan ScanningTime => _times[2];
        public TimeSpan ClientScanningTime => _times[3];
        public TimeSpan DefragmentationTime => _times[4];
        public TimeSpan PlacingTime => _times[5];
        public TimeSpan InventorizationTime => _times[6];
        public TimeSpan ShipmentTime => _times[7];
        public TimeSpan NonProductiveTime => _times[8];
        public TimeSpan TotalWorkingTime => _times[9];

        public int Lenght => _times.Length;

        public WorkingTime (IEnumerable<TimeSpan> times)
        {
            if (times == null) throw new ArgumentNullException();
            var timeArray = times.ToArray();

            _times = new TimeSpan[10];

            int i;

            for (i = 0; i < timeArray.Length && i < 9; ++i) {
                _times[i] = timeArray[i];
            }

            while (i < 9) {
                _times[i++] = TimeSpan.Zero;
            }

            _times[9] = _times.Sum();
        }
    }
}
