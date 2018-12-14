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
        public KeyValuePair< Dictionary< string, int>, Type >  GetTypeWithMap( SheetTable sheetTable )
        {
            throw new NotImplementedException();
        }
    }
}
