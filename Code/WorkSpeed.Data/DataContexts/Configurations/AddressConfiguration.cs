
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration< Address >
    {
        public void Configure ( EntityTypeBuilder< Address > builder )
        {
            builder.ToTable( "Addresses", "dbo" );

            builder.HasKey( a => new { a.Letter, a.Row, a.Section, a.Shelf, CellNum = a.Box } );

            builder.Property( p => p.Letter ).HasColumnType( "varchar(1)" ).IsRequired();
            builder.Property( p => p.Section ).HasColumnType( "tinyint" ).IsRequired();
            builder.Property( p => p.Shelf ).HasColumnType( "tinyint" ).IsRequired();
            builder.Property( p => p.Box ).HasColumnType( "tinyint" ).IsRequired();

            var converter = new EnumToStringConverter< BoxTypes >();
            builder.Property( p => p.BoxType ).HasConversion( converter ).HasColumnType( "varchar(50)" ).IsRequired( true );

            builder.Property( p => p.Length ).HasColumnType( "real" );
            builder.Property( p => p.Width ).HasColumnType( "real" );
            builder.Property( p => p.Height ).HasColumnType( "real" );

            builder.Property( p => p.Volume ).HasColumnType( "float" ).HasComputedColumnSql( "[Width] * [Length] * [Height]" );

            builder.Property( p => p.MaxWeight ).HasColumnType( "real" );
            builder.Property( p => p.VolumeCoefficient ).HasColumnType( "real" );
            builder.Property( p => p.Complexity ).HasColumnType( "real" );

            builder.HasOne( p => p.Position ).WithMany();

            // shadows
        }
    }
}
