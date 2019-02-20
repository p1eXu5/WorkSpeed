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

        public IEnumerable< (int,Category) > GetScans ( IEnumerable< Category > categories )
        {
            return new List< (int,Category) > {
                (10, new Category{ Name = "Category 1" }), 
                (200, new Category{ Name = "Category 2" }), 
                (50, new Category{ Name = "Category 3" }), 
                (150, new Category{ Name = "Category 4" }), 
                (5, new Category{ Name = "Category 5" }), 
                (25, new Category{ Name = "Category 6" }), 
            };
        }

        public IEnumerable< (int,Category) > GetLines ( IEnumerable< Category > categories )
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
