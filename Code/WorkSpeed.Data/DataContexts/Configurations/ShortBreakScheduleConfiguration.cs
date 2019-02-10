using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class ShortBreakScheduleConfiguration : IEntityTypeConfiguration< ShortBreakSchedule >
    {
        public void Configure ( EntityTypeBuilder< ShortBreakSchedule > builder )
        {
            builder.ToTable( "ShortBreaks", "dbo" );

            builder.HasKey( b => b.Id );
            builder.Property( b => b.Id ).UseSqlServerIdentityColumn();

            builder.Property( b => b.Name ).HasColumnType( "varchar(25)" ).IsRequired();
            builder.Property( b => b.Duration ).HasColumnType( "time" );
            builder.Property( b => b.Periodicity ).HasColumnType( "time" );
            builder.Property( b => b.FirstBreakTime ).HasColumnType( "time" );
            builder.Property( b => b.IsForSmokers ).HasColumnType( "bit" );

            builder.HasData( new ShortBreakSchedule[] {
                new ShortBreakSchedule { Id = 1, Name = "Перекуры для некурящих", Duration = TimeSpan.FromMinutes( 10 ), Periodicity = TimeSpan.FromHours( 2 ), IsForSmokers = false, },
                new ShortBreakSchedule {
                    Id = 2,
                    Name = "Перекуры для курящих",
                    Duration = TimeSpan.FromMinutes( 5 ),
                    Periodicity = TimeSpan.FromHours( 1 ),
                    IsForSmokers = true,

                },
            } );
        }
    }
}
