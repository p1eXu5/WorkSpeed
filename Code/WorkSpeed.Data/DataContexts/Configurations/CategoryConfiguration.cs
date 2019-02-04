
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration< Category >
    {
        public void Configure ( EntityTypeBuilder< Category > builder )
        {
            builder.ToTable( "Categories", "dbo" );

            builder.HasKey( c => c.Id );
            builder.Property( c => c.Id ).UseSqlServerIdentityColumn();

            builder.Property( c => c.Name ).HasColumnType( "varchar(50)" ).IsRequired();
            builder.Property( c => c.Abbreviation ).HasColumnType( "varchar(16)" ).IsRequired();
            builder.Property( c => c.MinVolume ).HasColumnType( "float" ).IsRequired();
            builder.Property( c => c.MaxVolume ).HasColumnType( "float" ).IsRequired();
        }
    }
}
