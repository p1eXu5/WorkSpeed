using System;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Productivity
{
    public struct Productivity
    {
        public Employee Employee { get; set; }

        public TimeSpan TotalTime { get; set; }
        public TimeSpan OffTime { get; set; }

        public Dictionary< OperationGroups, TimeSpan > OperationTimes { get; set; }

        public Dictionary< OperationGroups, Dictionary< Category, int >> Lines { get; set; }
        public Dictionary< OperationGroups, Dictionary<Category, int>> Quantities { get; set; }
        public Dictionary< OperationGroups, Dictionary<Category, int>> Scans { get; set; }
        public Dictionary< OperationGroups, Dictionary<Category, double>> Weight { get; set; }
        public Dictionary< OperationGroups, Dictionary<Category, double>> Volume { get; set; }
        public Dictionary<TimeSpan, int> Pauses { get; set; }

        public double GetSpeedLinesPerHour ( OperationGroups operation )
        {
            if ( !Lines.ContainsKey( operation ) ) return 0.0;

            return Lines[ operation ].Values.Sum() / OperationTimes[ operation ].TotalHours;
        }

        public int GetTotalLines ( OperationGroups operation )
        {
            if ( !Lines.ContainsKey( operation ) ) return 0;

            return Lines[ operation ].Values.Sum();
        }

        public double GetSpeedQuantityPerHour ( OperationGroups operation )
        {
            if ( !Quantities.ContainsKey( operation ) ) return 0.0;

            return Quantities[ operation ].Values.Sum() / OperationTimes[ operation ].TotalHours;
        }

        public int GetTotalQuantity ( OperationGroups operation )
        {
            if ( !Quantities.ContainsKey( operation ) ) return 0;

            return Quantities[ operation ].Values.Sum();
        }

        public double GetSpeedScansPerHour ( OperationGroups operation )
        {
            if ( !Scans.ContainsKey( operation ) ) return 0.0;

            return Scans[ operation ].Values.Sum() / OperationTimes[ operation ].TotalHours;
        }

        public int GetTotalScans ( OperationGroups operation )
        {
            if ( !Scans.ContainsKey( operation ) ) return 0;

            return Scans[ operation ].Values.Sum();
        }

        public double GetSpeedWeightPerHour ( OperationGroups operation )
        {
            if ( !Weight.ContainsKey( operation ) ) return 0.0;

            return Weight[ operation ].Values.Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetTotalWeight ( OperationGroups operation )
        {
            if ( !Weight.ContainsKey( operation ) ) return 0.0;

            return Weight[ operation ].Values.Sum();
        }

        public double GetSpeedVolumePerHour ( OperationGroups operation )
        {
            if ( !Volume.ContainsKey( operation ) ) return 0.0;

            return Volume[ operation ].Values.Sum() / OperationTimes[ operation ].TotalHours;
        }

        public double GetTotalVolume ( OperationGroups operation )
        {
            if ( !Volume.ContainsKey( operation ) ) return 0.0;

            return Volume[ operation ].Values.Sum();
        }
    }
}
