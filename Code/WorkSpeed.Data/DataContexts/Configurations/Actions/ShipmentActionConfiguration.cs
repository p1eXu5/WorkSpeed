﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.DataContexts.Configurations.Actions
{
    public class ShipmentActionConfiguration : IEntityTypeConfiguration< ShipmentAction >
    {
        public void Configure ( EntityTypeBuilder< ShipmentAction > builder )
        {
            builder.ToTable( "ShipmentActions", "dbo" );

            builder.HasKey( p => new { p.Id, p.EmployeeId } );
            builder.Property( p => p.Id ).HasColumnType( "varchar(11)" ).ValueGeneratedNever();
            builder.Property( p => p.EmployeeId ).HasColumnType( "varchar(7)" ).ValueGeneratedNever();
            builder.Property( p => p.DocumentName ).HasColumnType( "varchar(100)" );

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();

            builder.Property( d => d.Weight ).HasColumnType( "real" );
            builder.Property( d => d.Volume ).HasColumnType( "real" );
            builder.Property( d => d.ClientCargoQuantity ).HasColumnType( "real" );
            builder.Property( d => d.CommonCargoQuantity ).HasColumnType( "real" );

            builder.HasOne( p => p.Employee ).WithMany().HasForeignKey( p => p.EmployeeId ).IsRequired();
            builder.HasOne( p => p.Operation ).WithMany().IsRequired();
        }
    }
}
