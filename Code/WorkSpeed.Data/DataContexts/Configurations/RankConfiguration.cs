
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class RankConfiguration : IEntityTypeConfiguration< Rank >
    {
        public void Configure ( EntityTypeBuilder< Rank > builder )
        {
            builder.ToTable( "Ranks", "dbo" );

            builder.HasKey( r => r.Number );
            builder.Property( r => r.Number ).ValueGeneratedNever();

            builder.Property( r => r.OneHourCost ).HasColumnType( "decimal" );
        }
    }
}
