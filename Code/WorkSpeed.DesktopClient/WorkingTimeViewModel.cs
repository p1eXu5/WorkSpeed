using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.DesktopClient.Attributes;
using WorkSpeed.Manager.Models;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient
{
    public class WorkingTimeViewModel : ViewModel
    {
        private readonly WorkingTime _workingTime;
        private double _timeLineLenght;

        public WorkingTimeViewModel(WorkingTime workingTime)
        {
            _workingTime = workingTime;
        }

        public TimeSpan GatheringTime => _workingTime.GatheringTime;
        [Segment]
        public double GatheringTimeLenght => _workingTime.GatheringTime.Milliseconds;

        public TimeSpan ClientGatheringTime => _workingTime.ClientGatheringTime;
        [Segment]
        public double ClientGatheringTimeLenght => _workingTime.ClientGatheringTime.Milliseconds;

        public TimeSpan ScanningTime => _workingTime.ScanningTime;
        [Segment]
        public double ScanningTimeLenght => _workingTime.ScanningTime.Milliseconds;

        public TimeSpan ClientScanningTime => _workingTime.ClientScanningTime;
        [Segment]
        public double ClientScanningTimeLenght => _workingTime.ClientScanningTime.Milliseconds;

        public TimeSpan DefragmentationTime => _workingTime.DefragmentationTime;
        [Segment]
        public double DefragmentationTimeLenght => _workingTime.DefragmentationTime.Milliseconds;

        public TimeSpan PlacingTime => _workingTime.PlacingTime;
        [Segment]
        public double PlacingTimeLenght => _workingTime.PlacingTime.Milliseconds;

        public TimeSpan InventorizationTime => _workingTime.InventorizationTime;
        [Segment]
        public double InventorizationTimeLenght => _workingTime.InventorizationTime.Milliseconds;

        public TimeSpan ShipmentTime => _workingTime.ShipmentTime;
        [Segment]
        public double ShipmentTimeLenght => _workingTime.ShipmentTime.Milliseconds;

        public TimeSpan NonProductiveTime => _workingTime.NonProductiveTime;
        [Segment]
        public double NonProductiveTimeLenght => _workingTime.NonProductiveTime.Milliseconds;

        [Distance]
        public TimeSpan TotalWorkingTime => _workingTime.TotalWorkingTime;
    }
}
