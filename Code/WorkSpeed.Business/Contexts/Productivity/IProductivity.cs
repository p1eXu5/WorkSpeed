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

        IEnumerable< (int,Category) > GetLines ( IEnumerable< Category > categories );
        IEnumerable< (int,Category) > GetScans ( IEnumerable< Category > categories );



        IEnumerable< int > GetQuantity ();
        IEnumerable< double > GetVolumes ();
        IEnumerable< double > GetWeigths ();

        TimeSpan GetTotalTime ();

    }
}
