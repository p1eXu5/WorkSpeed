using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService 
{
    public interface IEntityObservableCollection< T >
    {
        ReadOnlyObservableCollection< T > Entities { get; set; }
    }
}