using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Data.Tests.FunctionalTests
{
    [TestFixture]
    public class WorkSpeedDbContextFunctionalTests
    {
        [Test]
        public void CanCreateDatabase()
        {
            using (var context = new WorkSpeedDbContext()) {

                context.Database.Migrate();
            }
        }

        [TearDown]
        public void DeleteDataBase()
        {
            using (var context = new WorkSpeedDbContext()) {

                context.Database.EnsureDeleted();
            }
        }
    }
}
