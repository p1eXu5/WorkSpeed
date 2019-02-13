using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IImportService
    {
        Task ImportFromXlsxAsync ( string fileName, IProgress< (int, string) > progress, CancellationToken cancellationToken );
    }
}
