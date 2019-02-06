using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Factories;

namespace WorkSpeed.Business.BusinessContexts
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
