using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public struct ProductivityEmployee
    {
        public Employee Employee { get; set; }

        public TimeSpan TotalTime { get; set; }
        public TimeSpan OffTime { get; set; }

        public Dictionary< OperationGroups, TimeSpan > OperationTimes { get; set; }

        public Dictionary< OperationGroups, int[] > Lines { get; set; }
        public Dictionary< OperationGroups, int[] > Quantities { get; set; }
        public Dictionary< OperationGroups, int[] > Scans { get; set; }
        public Dictionary< OperationGroups, double[] > Weight { get; set; }
        public Dictionary< OperationGroups, double[] > Volume { get; set; }

        public double GetSpeedLinesPerHour ( OperationGroups operation )
        {
            if ( !Lines.ContainsKey( operation ) ) return 0.0;

            return Lines[ operation ].Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetSpeedQuantitiesPerHour ( OperationGroups operation )
        {
            if ( !Quantities.ContainsKey( operation ) ) return 0.0;

            return Quantities[ operation ].Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetSpeedScansPerHour ( OperationGroups operation )
        {
            if ( !Scans.ContainsKey( operation ) ) return 0.0;

            return Scans[ operation ].Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetSpeedWeightPerHour ( OperationGroups operation )
        {
            if ( !Weight.ContainsKey( operation ) ) return 0.0;

            return Weight[ operation ].Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetSpeedVolumePerHour ( OperationGroups operation )
        {
            if ( !Volume.ContainsKey( operation ) ) return 0.0;

            return Volume[ operation ].Sum() / OperationTimes[ operation ].TotalHours;
        }
    }
}
