
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;
using System;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration< Shift >
    {
        public void Configure ( EntityTypeBuilder< Shift > builder )
        {
            builder.ToTable( "Shifts", "dbo" );

            builder.HasKey( s => s.Id );
            builder.Property( s => s.Id ).UseSqlServerIdentityColumn();

            builder.Property( s => s.Name ).HasColumnType( "nvarchar(20)" ).IsRequired();
            builder.Property( s => s.StartTime ).HasColumnType( "time" );
            builder.Property( s => s.Duration ).HasColumnType( "time" );
            builder.Property( s => s.Lunch ).HasColumnType( "time" );
            builder.Property( s => s.RestTime ).HasColumnType( "time" );

            builder.HasData( new Shift[] {
                new Shift { Id = 1, Name = "Дневная смена", StartTime = new TimeSpan( 8, 0, 0), Duration = TimeSpan.FromHours( 12 ), Lunch = TimeSpan.FromMinutes( 30 ), RestTime = TimeSpan.FromMinutes( 60 ) },
                new Shift { Id = 2, Name = "Ночная смена", StartTime = new TimeSpan( 20, 0, 0), Duration = TimeSpan.FromHours( 12 ), Lunch = TimeSpan.FromMinutes( 30 ), RestTime = TimeSpan.FromMinutes( 60 ) },
            } );
        }
    }
}
