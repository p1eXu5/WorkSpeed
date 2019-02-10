using System.Collections.Generic;

namespace WorkSpeed.Data.Models
{
    public class CategorySet
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List< CategoryCategorySet > Categories { get; set; }
    }
}
