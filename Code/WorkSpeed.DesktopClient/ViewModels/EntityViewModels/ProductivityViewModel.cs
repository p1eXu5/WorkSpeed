using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.MvvmBaseLibrary;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class ProductivityViewModel : ViewModel
    {
        private readonly Productivity _productivity;

        public ProductivityViewModel ( Productivity productivity )
        {
            _productivity = productivity ?? throw new ArgumentNullException();
        }

        public string EmployeeName => _productivity.Employee.Name;
        public string Position => _productivity.Employee.Position.Name;
        public string Rank => _productivity.Employee.Rank.Number.ToString();
        public string IsSmoker => _productivity.Employee.IsSmoker ? "Да" : "Нет";

        public TimeSpan WorkTime => _productivity.GetWorkTime();
        public TimeSpan OffTime => _productivity.GetOffTime();

        public int GatheredLines => _productivity.GetGatheredLines();
        public double GatheringSpeed => _productivity.GetGatheringSpeed();
    }
}
