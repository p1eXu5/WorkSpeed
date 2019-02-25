using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IProductivity : IEnumerable< Period >
    {
        void Add ( EmployeeActionBase action, Period period );
        Period this[ EmployeeActionBase action ] { get; set; }


        TimeSpan GetTime ();

        double GetLinesPerHour ();
        double GetScansPerHour ();
        double GetTotalVolume ();
        (double client, double nonClient) GetCargoQuantity();
        IEnumerable< (int count,Category category) > GetLines ( IEnumerable< Category > categories );
        IEnumerable< (int count,Category category) > GetScans ( IEnumerable< Category > categories );
        IEnumerable< (int count,Category category) > GetQuantity ( IEnumerable< Category > categories );
        IEnumerable< (double count,Category category) > GetVolumes ( IEnumerable< Category > categories );

        IEnumerable< double > GetWeigths ();

        double GetTotalHours ();

    }
}
