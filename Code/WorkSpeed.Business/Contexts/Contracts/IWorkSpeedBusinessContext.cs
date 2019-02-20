using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IWorkSpeedBusinessContext
    {
        bool HasProducts();
        Task<bool> HasProductsAsync ();

        IEnumerable< Product > GetProducts();
        void AddProduct ( Product product );

        IEnumerable< DoubleAddressAction > GetGatheringActions ();
        IEnumerable< DoubleAddressAction > GetGatheringActions ( Employee employee );
        void AddGatheringAction ( DoubleAddressAction gatheringAction );

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
