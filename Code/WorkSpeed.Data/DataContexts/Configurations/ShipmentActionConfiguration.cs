using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class ShipmentActionConfiguration : IEntityTypeConfiguration< ShipmentAction >
    {
        public void Configure ( EntityTypeBuilder< ShipmentAction > builder )
        {
            builder.ToTable( "ShipmentActions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();
            builder.HasOne( p => p.Employee ).WithMany().IsRequired();
            builder.HasOne( p => p.Document1C ).WithMany().IsRequired();
            builder.HasOne( p => p.Operation ).WithMany().IsRequired();
            builder.HasOne( p => p.ShipmentActionDetail ).WithOne( d => d.ShipmentAction ).IsRequired();
        }
    }
}
