using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class MoveRequestedEventArgs : EventArgs
    {
        public MoveRequestedEventArgs ( Directions direction )
        {
            Direction = direction;
        }

        public Directions Direction { get; }
    }
}
