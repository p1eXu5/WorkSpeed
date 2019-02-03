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
    public class EmployeeConfiguration : IEntityTypeConfiguration< Employee >
    {
        public void Configure ( EntityTypeBuilder< Employee > builder )
        {
            builder.ToTable( "Operations", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).HasColumnType( "varchar(7)" );

            builder.Property( p => p.Name ).HasColumnType( "varchar(255)" ).IsRequired( true );

        }
    }
}
