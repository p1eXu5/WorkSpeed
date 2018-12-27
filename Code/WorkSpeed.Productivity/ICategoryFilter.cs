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
        List< Category > CategoryList { get; }

        void AddCategory ( Category category );
    }
}
