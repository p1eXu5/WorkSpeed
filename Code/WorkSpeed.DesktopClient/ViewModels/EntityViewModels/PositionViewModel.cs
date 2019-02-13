using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.EntityViewModels
{
    public class PositionViewModel : ViewModel
    {
        private readonly Position _position;

        public PositionViewModel ( Position position )
        {
            _position = position;
        }
    }
}
