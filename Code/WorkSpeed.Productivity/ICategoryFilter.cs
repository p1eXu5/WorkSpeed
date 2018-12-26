using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public interface ICategoryFilter
    {
        int GetCategory ( Product product );
        string GetCategoryName ( int category );

        int Count { get; }
    }
}
