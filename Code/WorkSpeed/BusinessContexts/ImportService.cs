using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using WorkSpeed.Business.BusinessContexts.Contracts;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Business.BusinessContexts
{
    public class ImportService : Service, IImportService
    {
        private readonly ITypeRepository _typeRepository;

        public ImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext )
        {
            _typeRepository = typeRepository;
        }

        public Task ImportFromXlsxAsync ( string fileName, IProgress< (int, string) > progress )
        {
            var task = new Task( () => ImportFromXlsx( fileName, progress ) );
            task.Start();

            return task;
        }

        public void ImportFromXlsx ( string fileName, IProgress< (int, string) > progress )
        {
            var table = ExcelImporter.GetSheetTable( fileName );
            var propertyMap = _typeRepository.GetTypeAndPropertyMap( table );

            if ( null == propertyMap.type ) {

                progress.Report( (-1, @"Не удалось прочитать файл. Файл либо открыт в другой программе, " 
                                      + @"либо содержит таблицу, тип которой определить не удалось.") );

                return;
            }


        }
    }
}
