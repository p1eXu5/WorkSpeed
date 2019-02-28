using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;

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

        [ Test ]
        public async Task TwoContexts ()
        {
            using (var context = new WorkSpeedDbContext()) {
                context.Database.Migrate();
            }

            using ( var context1 = new WorkSpeedDbContext() ) {
                using ( var context2 = new WorkSpeedDbContext() ) {

                    // create

                    var employee = new Employee { Id = "AR00001", Name = "Test Employee", IsActive = true };
                    context1.Add( employee );
                    context1.SaveChanges();

                    foreach ( var entityEntry in context2.ChangeTracker.Entries<Employee>() ) {
                        entityEntry.Reload();
                    }

                    var empl = context2.Employees.ToArray();
                    Assert.That( empl[0].Name, Is.EqualTo( employee.Name ) );
                    Assert.That( empl.Length, Is.EqualTo( 1 ) );


                    // update
                    var newName = "John Doe";

                    employee = context1.Employees.ToArray()[0];
                    employee.Name = newName;
                    context1.SaveChanges();
                    
                    employee = new Employee { Id = "AR00002", Name = "Test Employee", IsActive = true };
                    context1.Add( employee );
                    context1.SaveChanges();

                    var employees = context1.Employees.ToArray();
                    Assert.That( employees.Length, Is.EqualTo( 2 ) );


                    foreach (var entityEntry in context2.ChangeTracker.Entries<Employee>()) {
                        await entityEntry.ReloadAsync();
                    }

                    empl = context2.Employees.ToArray();
                    Assert.That( empl[0].Name, Is.EqualTo( newName ) );
                    Assert.That( empl.Length, Is.EqualTo( 2 ) );
                }
            }
        }

    }
}
