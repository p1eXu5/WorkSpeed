
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.Models
{
    public class Operation : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OperationGroups Group { get; set; }
        public float? Complexity { get; set; }
    }
}
