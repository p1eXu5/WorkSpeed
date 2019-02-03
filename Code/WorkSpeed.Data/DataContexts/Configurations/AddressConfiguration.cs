using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration< Address >
    {
        public void Configure ( EntityTypeBuilder< Address > builder )
        {
            builder.ToTable( "Addresses", "dbo" );

            builder.HasKey( a => new { a.Letter, a.Row, a.Section, a.Shelf, CellNum = a.Box } );

            builder.Property( p => p.Letter ).HasColumnType( "varchar(1)" ).IsRequired( true );
            builder.Property( p => p.Section ).HasColumnType( "tinyint" ).IsRequired( true );
            builder.Property( p => p.Shelf ).HasColumnType( "tinyint" ).IsRequired( true );
            builder.Property( p => p.Box ).HasColumnType( "tinyint" ).IsRequired( true );

            var converter = new EnumToStringConverter< BoxTypes >();
            builder.Property( p => p.BoxType ).HasConversion( converter ).HasColumnType( "varchar(50)" ).IsRequired( true );

            builder.Property( p => p.Lenght ).HasColumnType( "real" );
            builder.Property( p => p.Width ).HasColumnType( "real" );
            builder.Property( p => p.Height ).HasColumnType( "real" );

            builder.Property( p => p.Volume ).HasColumnType( "float" ).HasComputedColumnSql( "[Width] * [Length] * [Height]" );

            builder.Property( p => p.MaxWeight ).HasColumnType( "real" );
            builder.Property( p => p.VolumeCoefficient ).HasColumnType( "real" );
            builder.Property( p => p.Complexity ).HasColumnType( "real" );

            builder.HasOne( p => p.Position ).WithMany();
        }
    }
}
