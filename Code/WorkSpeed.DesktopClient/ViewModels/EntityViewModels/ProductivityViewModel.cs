using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string EmployeeName => _productivity.Employee.Name;
        public string Position => _productivity.Employee.Position.Name;
        public string Rank => _productivity.Employee.Rank.Number.ToString();
        public string IsSmoker => _productivity.Employee.IsSmoker ? "Да" : "Нет";


    }
}
