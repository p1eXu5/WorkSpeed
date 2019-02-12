using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.DataContexts.Configurations.Actions
{
    public class InventoryActionConfiguration : IEntityTypeConfiguration< InventoryAction >
    {
        public void Configure ( EntityTypeBuilder< InventoryAction > builder )
        {
            builder.ToTable( "InventoryActions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "varchar(11)" ).ValueGeneratedNever();
            builder.Property( p => p.DocumentName ).HasColumnType( "varchar(100)" );

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();

            builder.HasOne( p => p.Employee ).WithMany().IsRequired();
            builder.HasOne( p => p.Operation ).WithMany().IsRequired();

            builder.HasMany( p => p.InventoryActionDetails )
                   .WithOne( d => d.InventoryAction );
        }
    }
}
