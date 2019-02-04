
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class CategorySetConfiguration : IEntityTypeConfiguration< CategorySet >
    {
        public void Configure ( EntityTypeBuilder< CategorySet > builder )
        {
            builder.ToTable( "CategorySets", "dbo" );

            builder.HasKey( c => c.Id );
            builder.Property( c => c.Id ).UseSqlServerIdentityColumn();

            builder.Property( a => a.Name ).HasColumnType( "varchar(255)" );

            builder.HasOne( c => c.Category0 ).WithMany();
            builder.HasOne( c => c.Category1 ).WithMany();
            builder.HasOne( c => c.Category2 ).WithMany();
            builder.HasOne( c => c.Category3 ).WithMany();
            builder.HasOne( c => c.Category4 ).WithMany();
            builder.HasOne( c => c.Category5 ).WithMany();
            builder.HasOne( c => c.Category6 ).WithMany();
            builder.HasOne( c => c.Category7 ).WithMany();
            builder.HasOne( c => c.Category8 ).WithMany();
            builder.HasOne( c => c.Category9 ).WithMany();
        }
    }
}
