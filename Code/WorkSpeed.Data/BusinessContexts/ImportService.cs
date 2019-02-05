using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts.Contracts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.BusinessContexts
{
    public class ImportService : Service, IImportService
    {
        public ImportService ( WorkSpeedDbContext dbContext ) : base( dbContext ) { }

        public void ImportFromXlsxAsync ( string fileName, IProgress< (string, double) > progress )
        {
            throw new NotImplementedException();
        }
    }
}
