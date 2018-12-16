using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts
{
    public class WorkSpeedDataContext : DbContext
    {
        public WorkSpeedDataContext ()
        { }

        public WorkSpeedDataContext ( DbContextOptions< WorkSpeedDataContext > options )
            : base( options )
        { }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            if ( !optionsBuilder.IsConfigured ) {
                optionsBuilder.UseSqlServer( ConfigurationManager.ConnectionStrings[ "Default" ].ConnectionString );
            }
        }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity< Address >().HasKey( a => new { a.Letter, a.Row, a.Section, a.Shelf, a.CellNum } );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Document1C> Documents { get; set; }
        public DbSet<GatheringAction> GatheringActions { get; set; }
        public DbSet<ShipmentAction> ShipmentActions { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
