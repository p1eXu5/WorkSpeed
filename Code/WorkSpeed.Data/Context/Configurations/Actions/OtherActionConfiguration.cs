﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Context.Configurations.Actions
{
    public class OtherActionConfiguration : IEntityTypeConfiguration< OtherAction >
    {
        public void Configure ( EntityTypeBuilder< OtherAction > builder )
        {
            builder.ToTable( "OtherActions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "nvarchar(11)" ).ValueGeneratedNever();
            builder.Property( p => p.DocumentName ).HasColumnType( "nvarchar(100)" );

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();

            builder.HasOne( p => p.Employee ).WithMany( e => e.OtherActions ).OnDelete( DeleteBehavior.Cascade ).IsRequired();
            builder.HasOne( p => p.Operation ).WithMany().IsRequired();
        }
    }
}
