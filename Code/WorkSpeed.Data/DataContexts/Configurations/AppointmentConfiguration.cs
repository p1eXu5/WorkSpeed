
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

            builder.Property( a => a.OfficialName ).HasColumnType( "nvarchar(50)" );
            builder.Property( a => a.InnerName ).HasColumnType( "nvarchar(50)" ).IsRequired();
            builder.Property( a => a.SalaryPerOneHour ).HasColumnType( "decimal" );
            builder.Property( a => a.Abbreviations ).HasColumnType( "nvarchar(50)" ).IsRequired();

            builder.HasData( new Appointment[] {
                new Appointment { Id = 1, InnerName = "Грузчик", OfficialName = "Грузчик", SalaryPerOneHour = 47.85m, Abbreviations = "гр.;гр;груз;грузч.;" },
                new Appointment { Id = 2, InnerName = "Кладовщик на РРЦ", OfficialName = "Кладовщик склада", SalaryPerOneHour = 52.64m, Abbreviations = "кл.;кладовщик;кл;клад;клад.;" },
                new Appointment { Id = 3, InnerName = "Кладовщик приемщик", OfficialName = "Кладовщик-приемщик", SalaryPerOneHour = 57.42m, Abbreviations = "пр.;приёмщик;приемщик;пр;" },
                new Appointment { Id = 4, InnerName = "Водитель погрузчика", OfficialName = "Водитель погрузчика", SalaryPerOneHour = 52.64m, Abbreviations = "вод.;водитель;вод;карщик;" },
                new Appointment { Id = 5, InnerName = "Старший кладовщик на РРЦ", OfficialName = "Старший кладовщик склада", SalaryPerOneHour = 62.21m, Abbreviations = "ст.кл.;старший;ст;ст.;старшийкладовщик;ст.клад.;" },
                new Appointment { Id = 6, InnerName = "Заместитель управляющего склада по отгрузке", OfficialName = "Менеджер по отправке груза", SalaryPerOneHour = 95.70m, Abbreviations = "зам.пр.;зампоприёмке;зампр;" },
                new Appointment { Id = 7, InnerName = "Заместитель управляющего склада по приемке", OfficialName = "Менеджер по приему груза", SalaryPerOneHour = 92.22m, Abbreviations = "зам.отгр.;зампоотгрузке;замотгр;" },
                new Appointment { Id = 8, InnerName = "Управляющий РРЦ", OfficialName = "Управляющий складом", SalaryPerOneHour = 119.63m, Abbreviations = "упр.скл.;управляющий;упр.;упр;упр.складом;" },
            } );
        }
    }
}
