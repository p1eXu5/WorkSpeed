using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class ProductivityEmployee
    {
        public ProductivityEmployee Employee { get; set; }

        public TimeSpan TotalTime { get; set; }
        public TimeSpan OffTime { get; set; }

        public Dictionary< OperationGroups, TimeSpan > OperationTimeDictionary { get; set; }

        public Dictionary< OperationGroups, int[] > Lines { get; set; }
        public Dictionary< OperationGroups, int[] > Quantities { get; set; }
        public Dictionary< OperationGroups, int[] > Scans { get; set; }
        public Dictionary< OperationGroups, double[] > Weight { get; set; }
        public Dictionary< OperationGroups, double[] > Volume { get; set; }

        public double GetSpeedLinesPerHour ( OperationGroups operation )
        {
            throw new NotImplementedException();
        }

        public double GetSpeedQuantitiesPerHour ( OperationGroups operation )
        {
            throw new NotImplementedException();
        }

        public double GetSpeedScansPerHour ( OperationGroups operation )
        {
            throw new NotImplementedException();
        }

        public double GetSpeedWeightPerHour ( OperationGroups operation )
        {
            throw new NotImplementedException();
        }

        public double GetSpeedVolumePerHour ( OperationGroups operation )
        {
            throw new NotImplementedException();
        }
    }
}
