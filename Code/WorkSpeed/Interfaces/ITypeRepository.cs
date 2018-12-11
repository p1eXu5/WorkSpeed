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
        ArrayList GetTypeCollection (SheetTable sheetTable, Type[] includingAttributes, Type[] excludingAttributes);
    }
}
