using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Business.FileModels;

namespace WorkSpeed.Business.Tests.Contexts.IntegrationTests
{
    public class TestImportModel : ImportModel
    {
        [ Header( "Дата" )]
        public DateTime StartTime { get; set; }

        [ Header( "Операция" ) ]
        public string Operation { get; set; }

        [ Header( "КоличествоФакт" )]
        public int Quantity { get; set; }

        [ Header( "ВремяОперации_Сек" ) ]
        public int Duration { get; set; }
    }
}
