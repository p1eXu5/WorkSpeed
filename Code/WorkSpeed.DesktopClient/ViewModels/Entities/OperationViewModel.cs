using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Entities
{
    public class OperationViewModel
    {
        private readonly Operation _operation;

        public OperationViewModel ( Operation operation )
        {
            _operation = operation;
        }

        public string Name => _operation.Name;
    }
}
