﻿
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Context.Configurations;
using WorkSpeed.Data.Context.Configurations.ActionDetails;
using WorkSpeed.Data.Context.Configurations.Actions;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Context
{
    public class WorkSpeedDbContext : DbContext
    {
        public WorkSpeedDbContext ()
        { }

        public WorkSpeedDbContext ( DbContextOptions< WorkSpeedDbContext > options )
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
            modelBuilder.ApplyConfiguration( new PositionConfiguration() )
                        .ApplyConfiguration( new RankConfiguration() )
                        .ApplyConfiguration( new AppointmentConfiguration() )
                        .ApplyConfiguration( new ShiftConfiguration() )
                        .ApplyConfiguration( new ShortBreakScheduleConfiguration() );

            modelBuilder.ApplyConfiguration( new AvatarConfiguration() )
                        .ApplyConfiguration( new EmployeeConfiguration() );

            modelBuilder.ApplyConfiguration( new ProductConfiguration() )
                        .ApplyConfiguration( new AddressConfiguration() )
                        .ApplyConfiguration( new OperationConfiguration() );

            modelBuilder.ApplyConfiguration( new InventoryActionConfiguration() )
                        .ApplyConfiguration( new ReceptionActionConfiguration() )
                        .ApplyConfiguration( new DoubleAddressActionConfiguration() )
                        .ApplyConfiguration( new ShipmentActionConfiguration() )
                        .ApplyConfiguration( new OtherActionConfiguration() );

            modelBuilder.ApplyConfiguration( new InventoryActionDetailConfiguration() )
                        .ApplyConfiguration( new ReceptionActionDetailConfiguration() )
                        .ApplyConfiguration( new DoubleAddressActionDetailConfiguration() );

            modelBuilder.ApplyConfiguration( new CategoryConfiguration() )
                        .ApplyConfiguration( new CategorySetConfiguration() )
                        .ApplyConfiguration( new CategoryCategorySetConfiguration() );
        }

        public DbSet< Position > Positions { get; set; }
        public DbSet< Rank > Ranks { get; set; }
        public DbSet< Appointment > Appointments { get; set; }
        public DbSet< Shift > Shifts { get; set; }
        public DbSet< ShortBreakSchedule > ShortBreakSchedules { get; set; }
        public DbSet< Avatar > Avatars { get; set; }

        public DbSet< Employee > Employees { get; set; }

        public DbSet< Product > Products { get; set; }
        public DbSet< Operation > Operations { get; set; }
        public DbSet< Address > Addresses { get; set; }

        public DbSet< InventoryAction > InventoryActions { get; set; }
        public DbSet< ReceptionAction > ReceptionActions { get; set; }
        public DbSet< DoubleAddressAction > DoubleAddressActions { get; set; }
        public DbSet< ShipmentAction > ShipmentActions { get; set; }
        public DbSet< OtherAction > OtherActions { get; set; }

        public DbSet< CategorySet > CategorySets { get; set; }
    }
}
