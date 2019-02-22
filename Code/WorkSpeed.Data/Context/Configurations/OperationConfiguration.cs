
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.Context.Configurations
{
    public class OperationConfiguration : IEntityTypeConfiguration< Operation >
    {
        public void Configure ( EntityTypeBuilder< Operation > builder )
        {
            builder.ToTable( "Operations", "dbo" );

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            builder.Property( p => p.Name ).HasColumnType( "nvarchar(50)" ).IsRequired();

            var converter = new EnumToStringConverter< OperationGroups >();
            builder.Property( p => p.Group ).HasConversion( converter ).HasColumnType( "varchar(10)" ).IsRequired();

            builder.Property( p => p.Complexity ).HasColumnType( "real" );

            builder.HasData( new Operation[] {
                new Operation { Id = 1, Name = "Сканирование товара",    Group = OperationGroups.Reception, Complexity = (float) 1.0 },
                new Operation { Id = 2, Name = "Сканирование транзитов", Group = OperationGroups.Reception, Complexity = (float) 1.0 },

                new Operation { Id = 3, Name = "Размещение товара",    Group = OperationGroups.Placing, Complexity = (float) 1.0 },
                new Operation { Id = 4, Name = "Перемещение товара",   Group = OperationGroups.Placing, Complexity = (float) 1.0 },
                new Operation { Id = 5, Name = "Подтоварка",           Group = OperationGroups.Defragmentation, Complexity = (float) 1.0 },
                new Operation { Id = 6, Name = "Верт. дефрагментация", Group = OperationGroups.Defragmentation, Complexity = (float) 1.0 },
                new Operation { Id = 7, Name = "Гор. дефрагментация",  Group = OperationGroups.Defragmentation, Complexity = (float) 1.0 },

                new Operation { Id = 8,  Name = "Подбор товара",                 Group = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 9,  Name = "Подбор клиентского товара",     Group = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 11, Name = "Предварительный подбор товара", Group = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 10, Name = "Подбор товаров покупателей",    Group = OperationGroups.BuyerGathering, Complexity = (float) 1.0 },
                new Operation { Id = 12, Name = "Упаковка товара в места",       Group = OperationGroups.Placing, Complexity = (float) 1.0 },

                new Operation { Id = 13, Name = "Инвентаризация",  Group = OperationGroups.Inventory, Complexity = (float?) 1.0 },

                new Operation { Id = 14, Name = "Выгрузка машины", Group = OperationGroups.Shipment, Complexity = (float) 1.0 },
                new Operation { Id = 15, Name = "Погрузка машины", Group = OperationGroups.Shipment, Complexity = (float) 1.0 },

                new Operation { Id = 16, Name = "Прочие операции", Group = OperationGroups.Other, Complexity = (float) 1.0 },
            } );
        }
    }
}
