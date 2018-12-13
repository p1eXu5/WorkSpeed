using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    /// <summary>
    /// Сбор товаров покупателей.
    /// </summary>
    public class ClientGatheringAction : WithProductAction
    {
        /// <summary>
        /// Может быть как ячейкой быстрого подбора, так и ячейкой хранения.
        /// </summary>
        public Address GatheringCellAdress { get; set; }
    }
}
