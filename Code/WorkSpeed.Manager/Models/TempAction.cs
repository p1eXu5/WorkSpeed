using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Manager.Models
{
    public class TempAction
    {
        public Employee Employee { get; }
        public Operation Operation { get; }
        public DateTime OperationStart { get; }

    }
}
