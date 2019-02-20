
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Context.Configurations.Actions
{
    public class DoubleAddressActionConfiguration : IEntityTypeConfiguration< DoubleAddressAction  >
    {
        public void Configure ( EntityTypeBuilder< DoubleAddressAction > builder )
        {
            builder.ToTable( "DoubleAddressActions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "nvarchar(11)" ).ValueGeneratedNever();
            builder.Property( p => p.DocumentName ).HasColumnType( "nvarchar(100)" );

            builder.Property( p => p.StartTime ).HasColumnType( "datetime2" ).IsRequired();
            builder.Property( p => p.Duration ).HasColumnType( "time" ).IsRequired();

            builder.HasOne(p => p.Employee).WithMany( e => e.DoubleAddressActions).OnDelete( DeleteBehavior.Cascade ).IsRequired();
            builder.HasOne(p => p.Operation).WithMany().IsRequired();

            builder.HasMany(p => p.DoubleAddressDetails)
                   .WithOne(d => d.DoubleAddressAction);
        }
    }
}
