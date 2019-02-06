using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.DataContexts.Configurations.ActionDetails
{
    public class DoubleAddressDetailConfiguration : IEntityTypeConfiguration< DoubleAddressDetail >
    {
        public void Configure ( EntityTypeBuilder< DoubleAddressDetail > builder )
        {
            builder.ToTable( "DoubleAddressDetails", "dbo" );

            builder.HasKey( d => d.Id );
            builder.Property( d => d.Id ).UseSqlServerIdentityColumn();

            builder.HasOne( d => d.SenderCellAdress )
                   .WithMany()
                   .IsRequired().OnDelete( DeleteBehavior.ClientSetNull );

            builder.HasOne( d => d.ReceiverCellAdress )
                   .WithMany()
                   .IsRequired().OnDelete( DeleteBehavior.ClientSetNull );

            builder.HasOne( d => d.DoubleAddressAction )
                   .WithMany( a => a.DoubleAddressDetails )
                   .HasForeignKey( d => d.DoubleAddressActionId )
                   .IsRequired();
        }
    }
}
