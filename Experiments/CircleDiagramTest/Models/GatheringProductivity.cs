using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleDiagramTest.Models
{
    public class GatheringProductivity : IProductivity
    {
        public Operation Operation { get; } = Operation.Operations[ 0 ];

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

        public IEnumerable< int > GetScans ( IEnumerable< Category> castegories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< int > GetLines ( IEnumerable< Category> castegories )
        {
            return new List< int > {
                100, 200, 150, 50, 10, 5
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
