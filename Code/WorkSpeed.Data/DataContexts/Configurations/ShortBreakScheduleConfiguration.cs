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
    public class ShortBreakScheduleConfiguration : IEntityTypeConfiguration< ShortBreakSchedule >
    {
        public void Configure ( EntityTypeBuilder< ShortBreakSchedule > builder )
        {
            builder.ToTable( "ShortBreaks", "dbo" );

            builder.HasKey( b => b.Id );
            builder.Property( b => b.Id ).UseSqlServerIdentityColumn();

            builder.Property( b => b.Name ).HasColumnType( "varchar(25)" ).IsRequired();
            builder.Property( b => b.Duration ).HasColumnType( "time" );
            builder.Property( b => b.Periodicity ).HasColumnType( "time" );
            builder.Property( b => b.FirstBreakTime ).HasColumnType( "time" );
            builder.Property( b => b.IsForSmokers ).HasColumnType( "bit" );
        }
    }
}
