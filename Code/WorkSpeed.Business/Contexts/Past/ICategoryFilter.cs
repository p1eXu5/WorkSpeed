using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface ICategoryFilter
    {
        IEnumerable<Category> CategoryList { get; }

        void AddCategory ( Category category );

        int GetCategoryIndex ( Product product );

        bool ContainsVolume ( Category category );

        void FillHoles ();

        void UndoHoles ();

        bool HasHoles ();
    }
}
