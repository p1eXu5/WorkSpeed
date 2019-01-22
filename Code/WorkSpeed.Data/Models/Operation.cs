
namespace WorkSpeed.Data.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Complexity { get; set; }

        public int GroupId { get; set; }
        public OperationGroup Group { get; set; }
    }
}
