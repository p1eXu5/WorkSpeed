﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.Tests.FunctionalTests
{
    [TestFixture]
    public class WorkSpeedDbContextFunctionalTests
    {
        [TearDown]
        public void DeleteDataBase()
        {
            using (var context = new WorkSpeedDbContext()) {

                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public void CanCreateDatabase()
        {
            using (var context = new WorkSpeedDbContext()) {

                context.Database.Migrate();
            }
        }

    }
}
