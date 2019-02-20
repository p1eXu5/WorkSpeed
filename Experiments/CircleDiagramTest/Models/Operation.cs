using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.Models
{
    public class Operation
    {
        public static Operation[] Operations { get; }

        static Operation ()
        {
            Operations = new Operation[] {
                new Operation { Id = 1, Name = "Подбор товара", Abbreviation = "op.1;op1;", Group = OperationGroups.Gathering },
                new Operation { Id = 2, Name = "Прием товара", Abbreviation = "op.2;op2;", Group = OperationGroups.Reception },
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public OperationGroups Group { get; set; }
    }
}
