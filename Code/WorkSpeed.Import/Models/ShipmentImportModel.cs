﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Import.Attributes;

namespace WorkSpeed.Import.Models
{
    public class ShipmentImportModel : BaseImportModel
    {
        [Header("Операция")]                public string Operation { get; set; }

        [Header("Вес на сотрудника")]           public double WeightPerEmployee { get; set; }

        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantityt { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }
    }
}
