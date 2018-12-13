using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    /// <summary>
    /// Упаковка товара как клиентского так и общего.
    /// </summary>
    public class PackingAction : WithProductAction
    {
        public Address DynamicCellAdress { get; set; }
    }
}
