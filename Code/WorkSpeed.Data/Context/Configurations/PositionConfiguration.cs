
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration< Position >
    {
        public void Configure ( EntityTypeBuilder< Position > builder )
        {
            builder.ToTable( "Positions", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Name ).HasColumnType( "nvarchar(50)" ).IsRequired();
            builder.Property( p => p.Abbreviations ).HasColumnType( "nvarchar(50)").IsRequired();
            builder.Property( p => p.Complexity ).HasColumnType( "real" );

            builder.HasData( new Position[] {
                new Position { Id = 1, Name = "-", Abbreviations = "-;", Complexity = 1.0f },
                new Position { Id = 2, Name = "Склад", Abbreviations = "упр.;упр;склад;", Complexity = 1.0f },
                new Position { Id = 3, Name = "Приёмка", Abbreviations = "пр.;приемка;пр;", Complexity = 1.0f },
                new Position { Id = 4, Name = "Отгрузка", Abbreviations = "отгр.;отгр;от.;от;", Complexity = 1.0f },
                new Position { Id = 5, Name = "Водитель", Abbreviations = "вод.;водитель;вод;", Complexity = 1.0f },
                new Position { Id = 6, Name = "Старший смены, крупняк", Abbreviations = "ст.см.кр.;ссмкр;", Complexity = 1.0f },
                new Position { Id = 7, Name = "Крупняк", Abbreviations = "кр.;кр;", Complexity = 1.0f },
                new Position { Id = 8, Name = "Старший смены, мезонин", Abbreviations = "ст.см.мез.;ссммез;", Complexity = 1.0f },
                new Position { Id = 9, Name = "Мезонин, 1-й этаж", Abbreviations = "мез.1;мезонин1;мез1;", Complexity = 1.0f },
                new Position { Id = 10, Name = "Мезонин, 2-й этаж", Abbreviations = "мез.2;мезонин2;мез2;", Complexity = 1.0f },
                new Position { Id = 11, Name = "Мезонин, 3-й этаж", Abbreviations = "мез.3;мезонин3;мез3;", Complexity = 1.0f },
                new Position { Id = 12, Name = "Мезонин, 4-й этаж", Abbreviations = "мез.4;мезонин4;мез4;", Complexity = 1.0f },
                new Position { Id = 13, Name = "Расстановка", Abbreviations = "рас.;расстановка;рас;", Complexity = 1.0f },
                new Position { Id = 14, Name = "Дорогая", Abbreviations = "дор.;дор;", Complexity = 1.0f },
            } );
        }
    }
}
