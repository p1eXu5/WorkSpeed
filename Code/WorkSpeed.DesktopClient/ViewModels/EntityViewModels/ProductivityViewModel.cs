using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.MvvmBaseLibrary;
using WorkSpeed.Productivity;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ProductivityViewModel : ViewModel
    {
        private readonly ProductivityEmployee _productivity;

        public ProductivityViewModel ( ProductivityEmployee productivity )
        {
            _productivity = productivity;


        }

        public string Name => _productivity.Employee.Name;
        public string Appointment => _productivity.Employee.Appointment.InnerName;
        public string Position => _productivity.Employee.Position.Name;
        public string Rank => _productivity.Employee.Rank.Number.ToString();
        public string IsSmoker => _productivity.Employee.IsSmoker ? "Да" : "Нет";

        public TimeSpan TotalTime => _productivity.TotalTime;
        public TimeSpan OffTime => _productivity.OffTime;
        public double GatheringSpeed => _productivity.GetSpeedLinesPerHour( OperationGroups.Gathering );
        public int GatheringQuantity => _productivity.Quantities[ OperationGroups.Gathering ].Sum();
        public Dictionary<string, >
    }
}
