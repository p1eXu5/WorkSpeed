using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed.Interfaces
{
    public interface IWarehouse
    {
        IWarehouseEntities NewData { get; }

        Task<bool> ImportAsync (string fileName);
        Task<bool> ImportAsync< TImportModel > ( string fileName, CancellationToken cancellationToken, IProgress<double> progress = null ) where TImportModel : ImportModel;
        Task<bool> HasProductsAsync ();

        IEnumerable< Product > GetProducts ();
        IEnumerable< Employee > GetEmployees ();
        IEnumerable<Appointment> GetAppointments ();
        IEnumerable<Position> GetPositions ();
        IEnumerable<Rank> GetRanks ();
        IEnumerable< GatheringAction > GetGatheringActions ();

        Task GetProductivitiesAsync ( Progress< Productivity > progress );
    }
}
