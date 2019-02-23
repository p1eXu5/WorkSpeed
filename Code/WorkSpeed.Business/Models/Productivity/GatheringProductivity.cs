using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public class GatheringProductivity
    {
        public Operation Operation { get; }

        public double GetLinesPerHour ()
        {
            return 154.8;
        }

        public double GetScansPerHour ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< int > GetQuantity ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int,Category) > GetScans ( IEnumerable< Category> castegories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int,Category) > GetLines ( IEnumerable< Category> castegories )
        {
            return new List< (int,Category) > {
                (60, new Category{ Name = "Category 1" }), 

            };
        }

        public IEnumerable< double > GetVolumes ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< double > GetWeigths ()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetTotalTime ()
        {
            throw new NotImplementedException();
        }
    }
}
