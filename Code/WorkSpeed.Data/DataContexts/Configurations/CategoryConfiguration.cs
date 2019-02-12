
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

            builder.Property( c => c.Name ).HasColumnType( "nvarchar(50)" ).IsRequired();
            builder.Property( c => c.Abbreviation ).HasColumnType( "nvarchar(16)" ).IsRequired();
            builder.Property( c => c.MinVolume ).HasColumnType( "float" );
            builder.Property( c => c.MaxVolume ).HasColumnType( "float" );

            builder.HasData( new Category[] {
                new Category { Id = 1, Name = "Товары до 2,5 литров", Abbreviation = "кат.1", MaxVolume = 2.5, },
                new Category { Id = 2, Name = "Товары до 5 литров", Abbreviation = "кат.2", MinVolume = 2.5, MaxVolume = 5.0, },
                new Category { Id = 3, Name = "Товары до 25 литров", Abbreviation = "кат.3", MinVolume = 5.0, MaxVolume = 25.0, },
                new Category { Id = 4, Name = "Товары до 100 литров", Abbreviation = "кат.4", MinVolume = 25.0, MaxVolume = 100.0, },
                new Category { Id = 5, Name = "Товары до 250 литров", Abbreviation = "кат.5", MinVolume = 100.0, MaxVolume = 250.0, },
                new Category { Id = 6, Name = "Товары от 250 литров", Abbreviation = "кат.6", MinVolume = 250.0, },
            } );
        }
    }
}
