using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;
using WorkSpeed.Interfaces;

namespace WorkSpeed
{
    public class ImportActionTypeRepository : ITypeRepository
    {
        public ArrayList GetTypeCollection (SheetTable sheetTable, Type[] includingAttributes, Type[] excludingAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
