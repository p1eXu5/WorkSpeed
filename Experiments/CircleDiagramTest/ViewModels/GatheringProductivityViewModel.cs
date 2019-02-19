﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class GatheringProductivityViewModel : CategorizedProductivityViewModel
    {
        public GatheringProductivityViewModel ( IProductivity productivity, IEnumerable< Category > categories ) 
            : base( productivity.Operation, categories )
        {
            SpeedLabeling = "стр./ч.";
            Speed = productivity.GetLinesPerHour();
            Aspects = productivity.GetLines( _categories ).Select( Convert.ToDouble ).ToArray();
        }

        
    }
}
