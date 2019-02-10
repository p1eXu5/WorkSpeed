
namespace WorkSpeed.Data.Models
{
    public class Position : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? Complexity { get; set; }
        public string Abbreviations { get; set; }
    }
}
