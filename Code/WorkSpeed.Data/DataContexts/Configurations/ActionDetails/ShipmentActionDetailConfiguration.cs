
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.DataContexts.Configurations.ActionDetails
{
    public class ShipmentActionDetailConfiguration : IEntityTypeConfiguration< ShipmentActionDetail >
    {
        public void Configure ( EntityTypeBuilder< ShipmentActionDetail > builder )
        {
            builder.ToTable( "ShipmentDetails", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Weight ).HasColumnType( "real" );
            builder.Property( p => p.Volume ).HasColumnType( "real" );
            builder.Property( p => p.ClientCargoQuantity ).HasColumnType( "real" );
            builder.Property( p => p.CommonCargoQuantity ).HasColumnType( "real" );

            builder.HasOne( p => p.ShipmentAction )
                   .WithOne( a => a.ShipmentActionDetail )
                   .HasForeignKey< ShipmentAction >( a => a.ShipmentDetailId )
                   .IsRequired();
        }
    }
}
