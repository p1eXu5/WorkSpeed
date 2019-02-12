using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;

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

            builder.HasData( new Operation[] {
                new Operation { Id = 1, Name = "Сканирование товара",    OperationGroup = OperationGroups.Reception, Complexity = (float) 1.0 },
                new Operation { Id = 2, Name = "Сканирование транзитов", OperationGroup = OperationGroups.Reception, Complexity = (float) 1.0 },

                new Operation { Id = 3, Name = "Размещение товара",    OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 4, Name = "Перемещение товара",   OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 5, Name = "Подтоварка",           OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 6, Name = "Верт. дефрагментация", OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 7, Name = "Гор. дефрагментация",  OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },

                new Operation { Id = 8,  Name = "Подбор товара",                 OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 9,  Name = "Подбор клиентского товара",     OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 10, Name = "Подбор товаров покупателей",    OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 11, Name = "Предварительный подбор товара", OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new Operation { Id = 12, Name = "Упаковка товара в места",       OperationGroup = OperationGroups.Gathering, Complexity = (float) 1.0 },

                new Operation { Id = 13, Name = "Инвентаризация",  OperationGroup = OperationGroups.Inventory, Complexity = (float?) 1.0 },

                new Operation { Id = 14, Name = "Выгрузка машины", OperationGroup = OperationGroups.Shipment, Complexity = (float) 1.0 },
                new Operation { Id = 15, Name = "Погрузка машины", OperationGroup = OperationGroups.Shipment, Complexity = (float) 1.0 },

                new Operation { Id = 16, Name = "Прочие операции", OperationGroup = OperationGroups.Other, Complexity = (float) 1.0 },
            } );
        }
    }
}
