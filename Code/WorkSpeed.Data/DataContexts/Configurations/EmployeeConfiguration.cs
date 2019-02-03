﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration< Employee >
    {
        public void Configure ( EntityTypeBuilder< Employee > builder )
        {
            builder.ToTable( "Employees", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "varchar(7)" );

            builder.Property( p => p.Name ).HasColumnType( "varchar(255)" ).IsRequired();
            builder.Property( p => p.IsActive ).HasColumnType( "bit" ).IsRequired();
            builder.Property( p => p.IsSmoker ).HasColumnType( "bit" ).IsRequired();
            builder.Property( p => p.ProbationEnd ).HasColumnType( "datetime2" ).IsRequired();

            builder.HasOne( p => p.Position ).WithMany().HasForeignKey( p => p.PositionId );
            builder.HasOne( p => p.Rank ).WithMany();
            builder.HasOne( p => p.Appointment ).WithMany();
            builder.HasOne( p => p.Shift ).WithMany();
            builder.HasOne( p => p.ShortBreakSchedule ).WithMany();

            builder.HasMany( p => p.ReceptionActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.DoubleAddressActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.InventoryActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.ShipmentActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.OtherActions ).WithOne( a => a.Employee );
        }
    }
}
