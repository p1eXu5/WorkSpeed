using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Manager.Models
{
    public struct Productivity
    {
        public Productivity(Employee employee)
        {
            Employee = employee;

            GatheringTime = TimeSpan.Zero;
            GatheredGoods = new HashSet<int>();

            PackagingTime = TimeSpan.Zero;
            PackagedGoods = new HashSet<int>();

            ClientGatheringTime = TimeSpan.Zero;
            ClientDeliveryTime = TimeSpan.Zero;
            ClientGatheredGoods = new HashSet<int>();
        }

        public readonly Employee Employee;

        public readonly TimeSpan GatheringTime;
        public readonly HashSet<int> GatheredGoods;

        public readonly TimeSpan PackagingTime;
        public readonly HashSet<int> PackagedGoods;

        public readonly TimeSpan ClientGatheringTime;
        public readonly TimeSpan ClientDeliveryTime;
        public readonly HashSet<int> ClientGatheredGoods;

        public readonly TimeSpan LoadingTime;
        public readonly double LoadingWeight;

        public readonly TimeSpan UnloadingTime;
        public readonly double UnloadingWeight;

        public readonly TimeSpan ScanningTime;
        public readonly HashSet<int> ScannedGoods;

        public readonly TimeSpan ClientScanningTime;
        public readonly HashSet<int> ClientScannedGoods;

        public readonly TimeSpan InventoringTime;
        public readonly HashSet<int> InventoredGoods;

        public readonly Queue<Address> Route;
    }

    public struct CategoryWeight
    {

    }
}
