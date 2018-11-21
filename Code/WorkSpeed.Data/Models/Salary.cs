using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        public ICollection<Employee> Employees { get; set; }

        public decimal OriginalBonus { get; set; }
        public decimal Holidays { get; set; }
        public decimal OriginalSalary { get; set; }
    }
}
