
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration< Product >
    {
        public void Configure ( EntityTypeBuilder< Product > builder )
        {
            builder.ToTable( "Products", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).ValueGeneratedNever();

            builder.Property( p => p.Name ).HasColumnType( "nvarchar(255)" ).IsRequired();
            builder.Property( p => p.IsGroup ).HasColumnType( "bit" ).IsRequired();
            builder.Property( p => p.ItemLength ).HasColumnType( "real" );
            builder.Property( p => p.ItemWidth ).HasColumnType( "real" );
            builder.Property( p => p.ItemHeight ).HasColumnType( "real" );

            builder.Property( p => p.ItemWeight ).HasColumnType( "real" );
            builder.Property( p => p.ItemVolume ).HasColumnType( "real" ).HasComputedColumnSql( "[ItemWidth] * [ItemLength] * [ItemHeight]" );

            builder.Property( p => p.CartonLength ).HasColumnType( "real" );
            builder.Property( p => p.CartonWidth ).HasColumnType( "real" );
            builder.Property( p => p.CartonHeight ).HasColumnType( "real" );

            builder.Property( p => p.CartonQuantity ).HasColumnType( "int" );
            
            builder.Property( p => p.CartonWeight ).HasColumnType( "real" ).HasComputedColumnSql( "[ItemWeight] * [CartonQuantity]" );
            builder.Property( p => p.CartonVolume ).HasColumnType( "real" ).HasComputedColumnSql( "[ItemWidth] * [ItemLength] * [ItemHeight] * [CartonQuantity]" );

            builder.Property( p => p.GatheringComplexity ).HasColumnType( "real" );
            builder.Property( p => p.InventoryComplexity ).HasColumnType( "real" );
            builder.Property( p => p.ScanningComplexity ).HasColumnType( "real" );
            builder.Property( p => p.PackagingComplexity ).HasColumnType( "real" );
            builder.Property( p => p.PlacingComplexity ).HasColumnType( "real" );

            builder.HasOne( p => p.Parent )
                   .WithMany( p => p.Children )
                   .HasForeignKey( p => p.ParentId )
                   .HasConstraintName( "ForeignKey_ProductChild_ProductParent" );
        }
    }
}
