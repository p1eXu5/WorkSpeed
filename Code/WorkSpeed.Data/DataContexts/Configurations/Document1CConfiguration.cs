
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class Document1CConfiguration : IEntityTypeConfiguration< Document1C >
    {
        public void Configure ( EntityTypeBuilder< Document1C > builder )
        {
            builder.ToTable( "Documents", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "varchar(10)" );

            builder.Property( p => p.Name ).HasColumnType( "varchar(50)" ).IsRequired();
            builder.Property( p => p.Date ).HasColumnType( "datetime2" ).IsRequired();
        }
    }
}
