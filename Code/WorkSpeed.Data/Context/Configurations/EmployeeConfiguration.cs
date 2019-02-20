
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration< Employee >
    {
        public void Configure ( EntityTypeBuilder< Employee > builder )
        {
            builder.ToTable( "Employees", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "nvarchar(7)" );

            builder.Property( p => p.Name ).HasColumnType( "nvarchar(255)" ).IsRequired();
            builder.Property( p => p.IsActive ).HasColumnType( "bit" ).IsRequired();
            builder.Property( p => p.IsSmoker ).HasColumnType( "bit" );
            builder.Property( p => p.ProbationEnd ).HasColumnType( "datetime2" );

            builder.HasOne( p => p.Avatar ).WithMany( a => a.Employees ).HasForeignKey( p => p.AvatarId );

            builder.HasOne( p => p.Position ).WithMany().HasForeignKey( p => p.PositionId );
            builder.HasOne( p => p.Rank ).WithMany();
            builder.HasOne( p => p.Appointment ).WithMany();
            builder.HasOne( p => p.Shift ).WithMany();
            builder.HasOne( p => p.ShortBreakSchedule ).WithMany();

            builder.HasMany( p => p.ReceptionActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.DoubleAddressActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.InventoryActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.ShipmentActions ).WithOne( a => a.Employee );
            builder.HasMany( p => p.OtherActions ).WithOne( a => a.Employee );
        }
    }
}
