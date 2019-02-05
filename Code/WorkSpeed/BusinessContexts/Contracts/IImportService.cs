using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.BusinessContexts.Contracts
{
    public interface IImportService
    {
        Task ImportFromXlsxAsync ( string fileName, IProgress<(string, int)> progress );
    }
}
