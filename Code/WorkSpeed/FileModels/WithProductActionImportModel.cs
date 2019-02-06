using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public abstract class WithProductActionImportModel : ActionImportModel
    {
        [ Header( "Код товара" ) ]
        public int ProductId { get; set; }

        [ Header( "Товар" ) ]
        public string Product { get; set; }

        [ Header( "Код родителя" ) ]
        [ Header( "Код родителя1" ) ]
        [ Header( "Родитель1Код" ) ]
        public int ImmadiateProductId { get; set; }

        [ Header( "Родитель1" ) ]
        public string ImmadiateProduct { get; set; }

        [ Header( "Код родителя 2" ) ]
        [ Header( "Родитель2Код" ) ]
        public int SecondProductId { get; set; }

        [ Header( "Родитель2" ) ]
        public string SecondProduct { get; set; }

        [ Header( "Количество" ) ]
        [ Header( "ФактическоеКоличество" ) ]
        public int ProductQuantity { get; set; }
    }
}
