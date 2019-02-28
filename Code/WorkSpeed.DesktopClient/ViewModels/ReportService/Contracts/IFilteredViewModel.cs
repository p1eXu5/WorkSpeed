using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Contracts
{
    public interface ICollectionViewList
    {
        List< ListCollectionView > ViewList { get; }
    }
}
