
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class CategorySetConfiguration : IEntityTypeConfiguration< CategorySet >
    {
        public void Configure ( EntityTypeBuilder< CategorySet > builder )
        {
            builder.ToTable( "CategorySets", "dbo" );

            builder.HasKey( c => c.Id );
            builder.Property( c => c.Id ).UseSqlServerIdentityColumn();

            builder.Property( a => a.Name ).HasColumnType( "nvarchar(50)" );

            builder.HasMany( c => c.Categories ).WithOne( ccs => ccs.CategorySet );

            builder.HasData( new CategorySet[] {
                new CategorySet { Id = 1, Name = "Категории Владивостока" }, 
            } );
        }
    }
}
