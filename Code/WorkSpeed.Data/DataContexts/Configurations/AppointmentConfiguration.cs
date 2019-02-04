
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration< Appointment >
    {
        public void Configure ( EntityTypeBuilder< Appointment > builder )
        {
            builder.ToTable( "Appointments", "dbo" );

            builder.HasKey( a => a.Id );
            builder.Property( a => a.Id ).UseSqlServerIdentityColumn();

            builder.Property( a => a.OfficialName ).HasColumnType( "varchar(255)" );
            builder.Property( a => a.InnerName ).HasColumnType( "varchar(255)" ).IsRequired();
            builder.Property( a => a.SalaryPerOneHour ).HasColumnType( "decimal" );
            builder.Property( a => a.Abbreviation ).HasColumnType( "varchar(16)" ).IsRequired();
        }
    }
}
