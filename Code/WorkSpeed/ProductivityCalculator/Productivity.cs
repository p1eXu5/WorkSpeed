using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.ProductivityCalculator
{
    public class Productivity
    {
        public Productivity (Employee employee)
        {
            Employee = employee ?? throw new ArgumentNullException();
        }

        public Employee Employee { get; }

        [Header("Время работы")]
        public ProductivityIndicators Times { get; set; } = new TimeIndicators();
        [Header("Собрано")]
        public ICollection<ProductivityIndicators> Gathered { get; set; } = new List<ProductivityIndicators>();
        [Header("Расставлено")]
        public ICollection<ProductivityIndicators> Placed { get; set; } = new List<ProductivityIndicators>();
        [Header("Подтоварено")]
        public ICollection<ProductivityIndicators> Defragmented { get; set; } = new List<ProductivityIndicators>();
        [Header("Проинвентарено")]
        public ICollection<ProductivityIndicators> Inventory { get; set; } = new List<ProductivityIndicators>();
        [Header("Просканировано")]
        public ICollection<ProductivityIndicators> Scanned { get; set; } = new List<ProductivityIndicators>();
        [Header("Загружено/Погружено")]
        public ICollection<ProductivityIndicators> LoadedUnloaded { get; set; } = new List<ProductivityIndicators>();
    }
}
