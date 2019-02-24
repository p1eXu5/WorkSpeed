using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IReportService
    {
        IEnumerable< Appointment > AppointmentCollection { get; }
        IEnumerable< Rank > RankCollection { get; }
        IEnumerable< Position > PositionCollection { get; }
        ReadOnlyObservableCollection< Shift > ShiftCollection { get; }
        ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakCollection { get; }
        IEnumerable< ShiftGrouping > ShiftGroupingCollection { get; }

        Task LoadEmployeesAsync ();
    }
}
