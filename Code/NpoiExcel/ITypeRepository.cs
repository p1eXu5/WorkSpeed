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
        /// <summary>
        /// Registers type of TType.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="includeAttribute"></param>
        /// <param name="excludeAttribute"></param>
        void RegisterType< TType >( Type includeAttribute = null, Type excludeAttribute = null );

        /// <summary>
        /// Registers type of type parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeAttribute"></param>
        /// <param name="excludeAttribute"></param>
        /// /// <exception cref="ArgumentNullException">When type is null.</exception>
        void RegisterType( Type type, Type includeAttribute = null, Type excludeAttribute = null );

        /// <summary>
        /// Returns tuple of Type and Dictionary&lt; propertyName, header &gt;
        /// </summary>
        /// <param name="sheetTable"><see cref="SheetTable"/></param>
        /// <returns>Tuple of Type and Dictionary&lt; propertyName, (header, int) &gt;</returns>
        (Type type, Dictionary< string, (string header, int column) > propertyMap) GetTypeWithMap ( SheetTable sheetTable );
    }
}
