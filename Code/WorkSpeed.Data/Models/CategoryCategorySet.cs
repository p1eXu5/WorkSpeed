namespace WorkSpeed.Data.Models
{
    public class CategoryCategorySet : IEntity
    {
        public int CategorySetId { get; set; }
        public CategorySet CategorySet { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
