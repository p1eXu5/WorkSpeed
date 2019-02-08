using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.DataContexts.Configurations.ActionDetails
{
    public class ReceptionActionDetailConfiguration : IEntityTypeConfiguration< ReceptionActionDetail >
    {
        public void Configure ( EntityTypeBuilder< ReceptionActionDetail > builder )
        {
            builder.ToTable( "ReceptionDetails", "dbo" );

            builder.HasKey( d => d.Id );
            builder.Property( d => d.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.ProductQuantity ).HasColumnType( "int" ).IsRequired();
            builder.HasOne( p => p.Product ).WithMany().HasForeignKey( p => p.ProductId ).IsRequired();

            builder.HasOne( d => d.Address ).WithMany().IsRequired();

            builder.Property( d => d.ScanQuantity ).HasColumnType( "smallint" ).IsRequired();
            builder.Property( d => d.IsClientScanning ).HasColumnType( "bit" ).IsRequired();

            builder.HasOne( d => d.ReceptionAction )
                   .WithMany( a => a.ReceptionActionDetails )
                   .HasForeignKey( d => d.ReceptionActionId )
                   .IsRequired();
        }
    }
}
