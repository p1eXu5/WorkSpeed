using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Constraints
{
    public interface ICategoryConstraints
    {
        int Count { get; }
        int GetCategoryNum ( Product product );
    }
}
