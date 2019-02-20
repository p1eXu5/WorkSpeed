
using Agbm.NpoiExcel;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Factories
{
    public static class WorkSpeedBusinessContextCreator
    {
        public static (ImportService,ReportService) Create ()
        {
            var typeRepo = GetTypeRepository();

            var context = new WorkSpeedDbContext();
            context.Database.Migrate();

            var importService = new ImportService( context, typeRepo );
            var reportService = new ReportService( new WorkSpeedDbContext() );

            return (importService, reportService);
        }

        private static ITypeRepository GetTypeRepository ()
        {
            var repo = new TypeRepository();

            repo.Fill();

            return repo;
        }
    }
}
