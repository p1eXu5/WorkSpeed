using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class CategorizedProductivityViewModel : ProductivityViewModel
    {
        protected Category[] _categories;

        public CategorizedProductivityViewModel ( Operation operation, IEnumerable< Category > categories ) : base( operation )
        {
            _categories = categories.ToArray();
        }
    }
}
