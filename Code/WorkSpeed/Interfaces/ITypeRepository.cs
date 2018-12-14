using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;

namespace WorkSpeed.Interfaces
{
    public interface ITypeRepository
    {
        KeyValuePair< Dictionary< string, int >, Type > GetTypeWithMap( SheetTable sheetTable );
        void RegisterType< TType >( Type propertyAttribute );
    }
}
