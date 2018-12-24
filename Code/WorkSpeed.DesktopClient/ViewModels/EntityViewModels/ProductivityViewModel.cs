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


    }
}
