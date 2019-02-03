using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.DataContexts.Configurations;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

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
            modelBuilder.ApplyConfiguration( new ProductConfiguration() )
                        .ApplyConfiguration( new PositionConfiguration() )
                        .ApplyConfiguration( new AddressConfiguration() );

            modelBuilder.ApplyConfiguration( new OperationConfiguration() );

            var converter = new EnumToStringConverter< OperationGroups >();

        }

        public DbSet< Employee > Employees { get; set; }
        public DbSet< Product > Products { get; set; }
        public DbSet< ReceptionAction > ReceptionActions { get; set; }
        public DbSet< DoubleAddressAction > DoubleAddressActions { get; set; }
        public DbSet< InventoryAction > InventoryActions { get; set; }
        public DbSet< ShipmentAction > ShipmentActions { get; set; }
        public DbSet< OtherAction > OtherActions { get; set; }
        public DbSet< Category > Categories { get; set; }
        public DbSet< Shift > Shifts { get; set; }
        public DbSet< ShortBreakSchedule > ShortBreakSchedules { get; set; }
    }
}
