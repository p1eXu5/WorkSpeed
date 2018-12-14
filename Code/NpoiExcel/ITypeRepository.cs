using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;

namespace NpoiExcel
{
    public interface ITypeRepository
    {
        (Type type, Dictionary< string, string > map) GetTypeWithMap ( SheetTable sheetTable );
        void RegisterType< TType >( Type propertyAttribute = null );
    }
}
