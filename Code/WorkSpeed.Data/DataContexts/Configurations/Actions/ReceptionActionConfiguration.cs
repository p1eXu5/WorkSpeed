﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.DataContexts.Configurations.Actions
{
    public class ReceptionActionConfiguration : IEntityTypeConfiguration< ReceptionAction >
    {
        public void Configure ( EntityTypeBuilder< ReceptionAction > builder )
        {
            builder.ToTable( "ReceptionActions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();

            builder.HasOne( p => p.Employee ).WithMany().IsRequired();
            builder.HasOne( p => p.Document1C ).WithMany().IsRequired();
            builder.HasOne( p => p.Operation ).WithMany().IsRequired();

            builder.HasMany( p => p.ReceptionActionDetails )
                   .WithOne( d => d.ReceptionAction );
        }
    }
}
