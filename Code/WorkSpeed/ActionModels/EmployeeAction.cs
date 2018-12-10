using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ActionModels
{ 
    public struct EmployeeAction
    {
        public readonly DateTime DateTime;
        public readonly Employee EmployeeId;
        public readonly byte OperationId;
        public readonly ushort DocumentId;
        public readonly IEnumerable<ImportedActionDetailsBase> ActionDetails;
        public readonly TimeSpan Duration;

        public EmployeeAction (DateTime dateTime, Employee employee, byte operationId, ushort documentId,
                               List<ImportedActionDetailsBase> details, TimeSpan duration)
        {
            DateTime = dateTime;
            EmployeeId = employee ?? throw new ArgumentNullException();
            OperationId = operationId;
            DocumentId = documentId;
            ActionDetails = details;
            Duration = duration;
        }
    }
}
