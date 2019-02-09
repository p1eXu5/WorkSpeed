
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration< Position >
    {
        public void Configure ( EntityTypeBuilder< Position > builder )
        {
            builder.ToTable( "Positions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Name ).HasColumnType( "varchar(50)" ).IsRequired();
            builder.Property( p => p.Abbreviation ).HasColumnType( "varchar(10)").IsRequired();
            builder.Property( p => p.Complexity ).HasColumnType( "real" );

            builder.HasData( new Position[] {
                new Position { Id = 1, Name = "Крупняк", Abbreviation = "кр.;кр;", Complexity = 1.0f },
                new Position { Id = 2, Name = "Приёмка", Abbreviation = "пр.;приемка;пр;", Complexity = 1.0f },
                new Position { Id = 3, Name = "Отгрузка", Abbreviation = "отгр.;отгр;от.;от;", Complexity = 1.0f },
                new Position { Id = 4, Name = "Дорогая", Abbreviation = "дор.;дор;", Complexity = 1.0f },
                new Position { Id = 5, Name = "Мезонин, 1-й этаж", Abbreviation = "мез.1;мезонин1;мез1;", Complexity = 1.0f },
                new Position { Id = 6, Name = "Мезонин, 2-й этаж", Abbreviation = "мез.2;мезонин2;мез2;", Complexity = 1.0f },
                new Position { Id = 7, Name = "Мезонин, 3-й этаж", Abbreviation = "мез.3;мезонин3;мез3;", Complexity = 1.0f },
                new Position { Id = 8, Name = "Мезонин, 4-й этаж", Abbreviation = "мез.4;мезонин4;мез4;", Complexity = 1.0f },
                new Position { Id = 9, Name = "Расстановка", Abbreviation = "рас.;расстановка;рас;", Complexity = 1.0f },
                new Position { Id = 10, Name = "Старший смены, мезонин", Abbreviation = "ст.см.мез.;ссммез;", Complexity = 1.0f },
                new Position { Id = 11, Name = "Старший смены, крупняк", Abbreviation = "ст.см.кр.;ссмкр;", Complexity = 1.0f },
            } );
        }
    }
}
