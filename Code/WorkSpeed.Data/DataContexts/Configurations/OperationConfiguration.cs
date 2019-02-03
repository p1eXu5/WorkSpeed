using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class OperationConfiguration : IEntityTypeConfiguration< Operation >
    {
        public void Configure ( EntityTypeBuilder< Operation > builder )
        {
            builder.ToTable( "Operations", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Name ).HasColumnType( "varchar(255)" ).IsRequired( true );

            var converter = new EnumToStringConverter< OperationGroups >();
            builder.Property( p => p.OperationGroup ).HasConversion( converter ).HasColumnType( "varchar(50)" ).IsRequired( true );

            builder.Property( p => p.Complexity ).HasColumnType( "real" );
        }
    }
}
