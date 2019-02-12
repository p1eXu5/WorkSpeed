using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public class ProductivityImportModel : ActionImportModel
    {
        [ Header( "Код товара" )]       public int? ProductId { get; set; }

        [ Header( "Товар" )]            public string Product { get; set; }

        [ Header( "Родитель1Код" )]
        [ Header( "Код родителя" )]
        [ Header( "Код родителя1" )]
                                        public int? ImmediateProductId { get; set; }

        [ Header( "Родитель1" ) ]
                                        public string ImmediateProduct { get; set; }

        [ Header( "Родитель2Код" ) ]
        [ Header( "Код родителя 2" ) ]
                                        public int? SecondProductId { get; set; }

        [ Header( "Родитель2" ) ]
                                        public string SecondProduct { get; set; }


        [Header("Количество учет")]          
        [Header("Учётное количество")]          
                                                public int? AccountingQuantity { get; set; }

        [ Header( "Количество факт" ) ]
        [ Header( "Количество" ) ]
        [ Header( "ФактическоеКоличество" ) ]
                                                public int? ProductQuantity { get; set; }

        [Header("Количество сканов")]           public int? ScanQuantity { get; set; }

        [Header("Сканирование транзитов")]      public bool? IsClientScanning { get; set; }

        [Header("Адрес отправитель")]           
        [Header("Адрес-отправитель")]           
                                                public string SenderAddress { get; set; }
        [Header("Адрес получатель")]            
        [Header("Адрес-получатель")]            
                                                public string ReceiverAddress { get; set; }

        [Header("Объем на сотрудника")]         public double? VolumePerEmployee { get; set; }

        [Header("Вес на сотрудника")]           public double? WeightPerEmployee { get; set; }

        [Header("Места ГМ на сотрудника")]       
        [Header("Номерные ГМ на сотрудника")]       
                                                    public double? ClientCargoQuantityt { get; set; }

        [Header("Места без ГМ на сотрудника")]       
        [Header("Безномерные ГМ на сотрудника")]    
                                                    public double? CommonCargoQuantity { get; set; }
    }
}
