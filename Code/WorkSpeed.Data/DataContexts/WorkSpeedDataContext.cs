using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            modelBuilder.Entity< Address >().HasKey( a => new { a.Letter, a.Row, a.Section, a.Shelf, CellNum = a.Box } );
            modelBuilder.Entity< Address >().Property( p => p.Volume ).HasComputedColumnSql( "[Width] * [Length] * [Height]" );

            modelBuilder.Entity< Product >().Property( p => p.ItemVolume ).HasComputedColumnSql( "[ItemWidth] * [ItemLength] * [ItemHeight]" );
            modelBuilder.Entity< Product >().Property( p => p.CartonWeight ).HasComputedColumnSql( "[ItemWeight] * [CartonQuantity]" );
            modelBuilder.Entity< Product >().Property( p => p.CartonVolume ).HasComputedColumnSql( "[ItemWidth] * [ItemLength] * [ItemHeight] * [CartonQuantity]" );

            modelBuilder.Entity< Operation >()
                        .HasOne( p => p.Group )
                        .WithMany( g => g.Operations )
                        .HasForeignKey( p => p.GroupId )
                        .HasConstraintName( "ForeignKey_Operation_OperationGroup" );

            var converter = new EnumToStringConverter< OperationGroups >();

            modelBuilder.Entity< OperationGroup >()
                        .Property( p => p.Name )
                        .HasConversion( converter );
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

    }
}
