using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Business.Factories;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Business.Contexts
{
    public static class WorkSpeedBusinessContextCreator
    {
        public static ImportService Create ()
        {
            var typeRepo = GetTypeRepository();

            var context = new WorkSpeedDbContext();
            context.Database.Migrate();

            var importService = new ImportService( context, typeRepo );

            return importService;
        }

        private static ITypeRepository GetTypeRepository ()
        {
            var repo = new TypeRepository();

            repo.Fill();

            return repo;
        }
    }
}
