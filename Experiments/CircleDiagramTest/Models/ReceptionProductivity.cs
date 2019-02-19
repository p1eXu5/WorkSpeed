using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleDiagramTest.Models
{
    public class ReceptionProductivity : IProductivity
    {
        public Operation Operation { get; } = Operation.Operations[ 1 ];

        public double GetLinesPerHour ()
        {
            throw new NotImplementedException();
        }

        public double GetScansPerHour ()
        {
            return 55.5;
        }

        public IEnumerable< int > GetQuantity ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< int > GetScans ( IEnumerable< Category > categories )
        {
            return new List< int > {
                10, 200, 50, 150, 5, 25
            };
        }

        public IEnumerable< int > GetLines ( IEnumerable< Category > categories )
        {
            throw new NotImplementedException();
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
