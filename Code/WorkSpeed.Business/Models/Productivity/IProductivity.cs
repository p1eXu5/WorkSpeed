using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public interface IProductivity
    {
        Operation Operation { get; }


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
