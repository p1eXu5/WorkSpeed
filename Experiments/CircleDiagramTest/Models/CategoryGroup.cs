using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleDiagramTest.Models
{
    public class CategoryGroup
    {
        public static List< Category > Categories => new List< Category > {
            new Category( 0, 50 ),
            new Category( 50, 100 )
        };
    }
}
