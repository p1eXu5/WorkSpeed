using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public interface IWorkSpeedBusinessContext
    {
        bool HasProducts();
        Task<bool> HasProductsAsync ();

        IEnumerable< Product > GetProducts();
        void AddProduct ( Product product );

        IEnumerable< GatheringAction > GetGatheringActions ();
        IEnumerable<GatheringAction> GetGatheringActions ( Employee employee );
        void AddGatheringAction ( GatheringAction gatheringAction );

        IEnumerable< Employee > GetEmployees ();
        void AddEmployee ( Employee employee );

        IEnumerable< Appointment > GetAppointments ();

        IEnumerable< Position > GetPositions ();

        IEnumerable< Rank > GetRanks ();

        IEnumerable< Category > GetCategories ();

        IEnumerable< Shift > GetShifts ();

        IEnumerable< ShortBreakSchedule > GetBreakList ();
    }
}
