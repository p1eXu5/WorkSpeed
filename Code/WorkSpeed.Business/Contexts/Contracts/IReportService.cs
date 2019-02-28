using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IReportService : IService
    {
        ReadOnlyObservableCollection< Appointment > AppointmentCollection { get; }
        ReadOnlyObservableCollection< Rank > RankCollection { get; }
        ReadOnlyObservableCollection< Position > PositionCollection { get; }
        ReadOnlyObservableCollection< Shift > ShiftCollection { get; }
        ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakCollection { get; }
        ReadOnlyObservableCollection< Operation > OperationCollection { get; }
        ReadOnlyObservableCollection< Category > CategoryCollection { get; }

        ReadOnlyObservableCollection< ShiftGrouping > ShiftGroupingCollection { get; }
        ReadOnlyObservableCollection< EmployeeProductivity > EmployeeProductivityCollections { get; }

        Task LoadShiftGroupingAsync ();
        Task LoadEmployeeProductivitiesAsync ( Period period );

        void UpdateRange ( IEnumerable< Employee > employees );
    }
}
