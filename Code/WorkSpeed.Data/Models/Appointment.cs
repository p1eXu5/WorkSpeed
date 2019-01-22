
namespace WorkSpeed.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string OfficialName { get; set; }
        public string InnerName { get; set; }
        public decimal SalaryPerOneHour { get; set; }
        public string Abbreviations { get; set; }
    }
}
