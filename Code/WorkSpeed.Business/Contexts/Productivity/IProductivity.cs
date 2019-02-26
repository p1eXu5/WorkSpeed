using System;
using System.Collections.Generic;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IProductivity : IEnumerable< Period >
    {
        Period this[ EmployeeActionBase action ] { get; set; }

        void Add ( EmployeeActionBase action, Period period );

        TimeSpan GetTime ();
        double GetLinesPerHour ();
        double GetScansPerHour ();
        double GetTotalVolume ();
        (double client, double nonClient) GetCargoQuantity();

        IEnumerable< (int count,Category category) > GetLines ( IEnumerable< Category > categories );
        IEnumerable< (int count,Category category) > GetScans ( IEnumerable< Category > categories );
        IEnumerable< (int count,Category category) > GetQuantity ( IEnumerable< Category > categories );
        IEnumerable< (double count,Category category) > GetVolumes ( IEnumerable< Category > categories );
    }
}
