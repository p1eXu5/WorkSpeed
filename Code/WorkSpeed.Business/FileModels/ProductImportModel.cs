﻿using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels
{
    public class ProductImportModel : ImportModel, IKeyedEntity< int >
    {
        [Header( "Код товара" )]        public int Id { get; set; }
        [Header( "Номенклатура" )]      public string Name { get; set; }

        [Header( "Колво в коробке" )]   public int? CartonQuantity { get; set; }

        [Header( "Вес ед" )]            public double? ItemWeight { get; set; }

        [Header( "Длина коробки" )]
        [Header( "ДлинаКоробки_см" )]
                                        public double? CartonLength { get; set; }

        [Header( "Ширина коробки" )]
        [Header( "ШиринаКоробки_см" )]
                                        public double? CartonWidth { get; set; }

        [Header( "Высота коробки" )]
        [Header( "ВысотаКоробки_см" )]
                                        public double? CartonHeight { get; set; }

        [Header( "ДлинаЕд_см" )]         public double? ItemLength { get; set; }
        [Header( "ШиринаЕд_см" )]         public double? ItemWidth { get; set; }
        [Header( "ВысотаЕд_см" )]         public double? ItemHeight { get; set; }
    }
}
