﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using WorkSpeed.Data.BusinessContexts.Contracts;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.BusinessContexts
{
    public class ImportService : Service, IImportService
    {
        private readonly ITypeRepository _typeRepository;

        public ImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext )
        {
            _typeRepository = typeRepository;
        }

        public Task ImportFromXlsxAsync ( string fileName, IProgress< (string, int) > progress )
        {
            throw new NotImplementedException();
        }
    }
}