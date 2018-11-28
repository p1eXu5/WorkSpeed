using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Manager.Models
{
    public class WarehouseConstraints
    {
        public Categories Categories { get; set; }

        public TimeSpan GetDurationWithoutBreaks (Operation beginOperation, Operation endOperation)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetDurationWithoutBreaks (Operation lastOperation)
        {
            throw new NotImplementedException();
        }
    }
}
