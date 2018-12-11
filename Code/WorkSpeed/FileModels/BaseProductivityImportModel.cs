using WorkSpeed.Attributes;

namespace WorkSpeed.FileModels
{
    public abstract class ActionProductivityImportModel : ActionImportModel
    {
        [Header("Код товара")]          public int ProductId { get; set; }
        [Header("Товар")]               public string Product { get; set; }
        [Header("Код родителя")]        public int ImmadiateProductId { get; set; }
        [Header("Родитель")]            public string ImmadiateProduct { get; set; }
        [Header("Код родителя 2")]      public int SecondProductId { get; set; }
        [Header("Родитель 2")]          public string SecondProduct { get; set; }
    }
}
