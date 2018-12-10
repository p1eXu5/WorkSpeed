using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Models.ImportModels
{
    public struct ImportedAction
    {
        public readonly DateTime DateTime;
        public readonly ushort EmployeeId;
        public readonly byte OperationId;
        public readonly ushort DocumentId;
        public readonly IEnumerable<ImportedActionDetailsBase> ActionDetails;
        public readonly TimeSpan Duration;

        public ImportedAction (DateTime dateTime, ushort employeeId, byte operationId, ushort documentId,
                               List<ImportedActionDetailsBase> details, TimeSpan duration)
        {
            DateTime = dateTime;
            EmployeeId = employeeId;
            OperationId = operationId;
            DocumentId = documentId;
            ActionDetails = details;
            Duration = duration;
        }
    }
}
