
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class RankConfiguration : IEntityTypeConfiguration< Rank >
    {
        public void Configure ( EntityTypeBuilder< Rank > builder )
        {
            builder.ToTable( "Ranks", "dbo" );

            builder.HasKey( r => r.Number );
            builder.Property( r => r.Number ).ValueGeneratedNever();

            builder.Property( r => r.OneHourCost ).HasColumnType( "decimal" );

            builder.HasData( new Rank[] {
                new Rank { Number = 2, OneHourCost = 163m },
                new Rank { Number = 3, OneHourCost = 180m },
                new Rank { Number = 4, OneHourCost = 200m },
                new Rank { Number = 5, OneHourCost = 220m },
                new Rank { Number = 6, OneHourCost = 242.42m },
                new Rank { Number = 7, OneHourCost = 266.67m },
                new Rank { Number = 8, OneHourCost = 300m },
                new Rank { Number = 9, OneHourCost = 342.42m },
                new Rank { Number = 10, OneHourCost = 366.66m },
                new Rank { Number = 11, OneHourCost = 400m },
                new Rank { Number = 12, OneHourCost = 442.42m },
                new Rank { Number = 13, OneHourCost = 466.66m },
                new Rank { Number = 14, OneHourCost = 533.33m },
            });
        }
    }
}
