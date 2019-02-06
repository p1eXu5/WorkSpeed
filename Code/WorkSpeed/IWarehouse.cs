using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Business.Interfaces;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business
{
    public interface IWarehouse
    {
        IWarehouseEntities NewData { get; }

        Task<bool> ImportAsync (string fileName);
        Task<bool> ImportAsync< TImportModel > ( string fileName, CancellationToken cancellationToken, IProgress<double> progress = null ) where TImportModel : ImportModel;
        Task<bool> HasProductsAsync ();
        void UpdateShiftBreaks ();

        void UpdateFixedBreaks ();

        IEnumerable< Product > GetProducts ();
        IEnumerable< Employee > GetEmployees ();
        IEnumerable< Appointment > GetAppointments ();
        IEnumerable< Position > GetPositions ();
        IEnumerable< Rank > GetRanks ();
        IEnumerable< Shift > GetShifts ();
        IEnumerable< ShortBreakSchedule > GetBreakList ();
        IEnumerable< DoubleAddressAction > GetGatheringActions ();
        IEnumerable< Category > GetCategories ();

        TimeSpan GetThreshold ();


    }
}
