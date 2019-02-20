
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.Context.Configurations.ActionDetails
{
    public class InventoryActionDetailConfiguration : IEntityTypeConfiguration< InventoryActionDetail >
    {
        public void Configure ( EntityTypeBuilder< InventoryActionDetail > builder )
        {
            builder.ToTable( "InventoryDetails", "dbo" );

            builder.HasKey( d => d.Id );
            builder.Property( d => d.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.ProductQuantity ).HasColumnType( "int" ).IsRequired();
            builder.HasOne( p => p.Product ).WithMany().HasForeignKey( p => p.ProductId ).IsRequired();

            builder.HasOne( d => d.Address ).WithMany().IsRequired();

            builder.Property( d => d.AccountingQuantity ).HasColumnType( "int" ).IsRequired();

            builder.HasOne( d => d.InventoryAction )
                   .WithMany( a => a.InventoryActionDetails )
                   .HasForeignKey( d => d.InventoryActionId )
                   .IsRequired();
        }
    }
}
