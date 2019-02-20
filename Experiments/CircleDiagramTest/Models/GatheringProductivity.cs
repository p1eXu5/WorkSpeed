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

        public IEnumerable< (int,Category) > GetScans ( IEnumerable< Category> castegories )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< (int,Category) > GetLines ( IEnumerable< Category> castegories )
        {
            return new List< (int,Category) > {
                (100, new Category{ Name = "Category 1" }), 
                (200, new Category{ Name = "Category 2" }), 
                (150, new Category{ Name = "Category 3" }), 
                (50, new Category{ Name = "Category 4" }), 
                (10, new Category{ Name = "Category 5" }), 
                (5, new Category{ Name = "Category 6" }), 
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
