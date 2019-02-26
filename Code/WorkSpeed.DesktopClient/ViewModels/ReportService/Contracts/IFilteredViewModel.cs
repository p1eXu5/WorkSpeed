using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Contracts
{
    public interface ICollectionViewList
    {
        List< ICollectionView > ViewList { get; }
    }
}
