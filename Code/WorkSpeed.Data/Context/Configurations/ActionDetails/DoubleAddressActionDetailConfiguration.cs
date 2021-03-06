﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.Context.Configurations.ActionDetails
{
    public class DoubleAddressActionDetailConfiguration : IEntityTypeConfiguration< DoubleAddressActionDetail >
    {
        public void Configure ( EntityTypeBuilder< DoubleAddressActionDetail > builder )
        {
            builder.ToTable( "DoubleAddressActionDetails", "dbo" );

            builder.HasKey( d => d.Id );
            builder.Property( d => d.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.ProductQuantity ).HasColumnType( "int" ).IsRequired();
            builder.HasOne( p => p.Product ).WithMany().HasForeignKey( p => p.ProductId ).IsRequired();

            builder.HasOne( d => d.SenderAddress )
                   .WithMany();

            builder.HasOne( d => d.ReceiverAddress )
                   .WithMany();

            //builder.HasOne( d => d.DoubleAddressAction )
            //       .WithMany( a => a.DoubleAddressDetails )
            //       .HasForeignKey( d => d.DoubleAddressActionId )
            //       .IsRequired();
        }
    }
}
