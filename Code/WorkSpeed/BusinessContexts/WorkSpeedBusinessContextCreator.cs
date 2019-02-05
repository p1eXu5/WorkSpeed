using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Factories;

namespace WorkSpeed.Data.BusinessContexts
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
