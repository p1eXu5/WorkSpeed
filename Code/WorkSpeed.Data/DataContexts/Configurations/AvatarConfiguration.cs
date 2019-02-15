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
    public class AvatarConfiguration : IEntityTypeConfiguration< Avatar >
    {
        public void Configure ( EntityTypeBuilder< Avatar > builder )
        {
            builder.ToTable("Avatars", "dbo");

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Picture ).HasColumnType( "varbinary" );
        }
    }
}
