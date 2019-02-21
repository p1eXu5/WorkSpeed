
using Agbm.NpoiExcel;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Productivity.Builders;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Contexts
{
    public static class WorkSpeedBusinessContextCreator
    {
        public static (ImportService,ReportService) Create ()
        {
            var typeRepo = GetTypeRepository();

            var context = new WorkSpeedDbContext();
            context.Database.Migrate();

            var importService = new ImportService( context, typeRepo );

            var builder = new ProductivityDirector();
            var reportService = new ReportService( new WorkSpeedDbContext(), builder );

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
