using NpoiExcel.Attributes;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class ProductImportModel : ImportModel
    {
        [Header( "Код товара" )]        public int Id { get; set; }
        [Header( "Номенклатура" )]      public string Name { get; set; }

        [Header( "Колво в коробке" )]   public int CartonQuantity { get; set; }

        [Header( "Вес ед" )]            public double Weight { get; set; }

        [Header( "Длина коробки" )]     public double CartonLength { get; set; }
        [Header( "Ширина коробки" )]    public double CartonWidth { get; set; }
        [Header( "Высота коробки" )]    public double CartonHeight { get; set; }

        [Header( "ОбъемЕд_л" )]         public double Volume { get; set; }

        [Header( "ДлинаЕд_см" )]         public double ItemLength { get; set; }
        [Header( "ШиринаЕд_см" )]         public double ItemWidth { get; set; }
        [Header( "ВысотаЕд_см" )]         public double ItemHeight { get; set; }


        public Product ToProduct( IImportModelVisitor visitor )
        {
            return visitor.ToProduct( this );
        }
    }
}
