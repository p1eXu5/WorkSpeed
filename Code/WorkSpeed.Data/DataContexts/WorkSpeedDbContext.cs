using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts
{
    public class WorkSpeedDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Document1C> Documents { get; set; }
        public DbSet<GatheringAction> GatheringActions { get; set; }
        public DbSet<ShipmentAction> ShipmentActions { get; set; }
    }
}
