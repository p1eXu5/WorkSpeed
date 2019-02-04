
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration< Position >
    {
        public void Configure ( EntityTypeBuilder< Position > builder )
        {
            builder.ToTable( "Positions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Name ).HasColumnType( "varchar(50)" ).IsRequired();
            builder.Property( p => p.Abbreviation ).HasColumnType( "varchar(10)").IsRequired();
            builder.Property( p => p.Complexity ).HasColumnType( "real" );
        }
    }
}
