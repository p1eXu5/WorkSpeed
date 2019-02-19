using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class TimeProductivityViewModel : ProductivityViewModel
    {
        public TimeProductivityViewModel ( Operation operation ) : base( operation ) { }
    }
}
