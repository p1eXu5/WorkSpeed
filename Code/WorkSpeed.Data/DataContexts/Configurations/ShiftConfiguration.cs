
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration< Shift >
    {
        public void Configure ( EntityTypeBuilder< Shift > builder )
        {
            builder.ToTable( "Shifts", "dbo" );

            builder.HasKey( s => s.Id );
            builder.Property( s => s.Id ).UseSqlServerIdentityColumn();

            builder.Property( s => s.Name ).HasColumnType( "varchar(20)" ).IsRequired();
            builder.Property( s => s.StartTime ).HasColumnType( "time" );
            builder.Property( s => s.ShiftDuration ).HasColumnType( "time" );
            builder.Property( s => s.LunchDuration ).HasColumnType( "time" );
            builder.Property( s => s.RestTime ).HasColumnType( "time" );
        }
    }
}
