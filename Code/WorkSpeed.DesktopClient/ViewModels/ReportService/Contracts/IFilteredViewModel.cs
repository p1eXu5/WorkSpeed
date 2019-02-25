using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public interface IFilteredViewModel
    {
        ICollectionView View { get; }
        void Refresh ();
    }
}
