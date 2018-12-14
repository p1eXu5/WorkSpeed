using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace NpoiExcel
{
    public class TypeRepository : ITypeRepository
    {
        private readonly Dictionary< Type, Dictionary< string[], string > > _typeDictionary = new Dictionary< Type, Dictionary< string[], string > >();

        /// <summary>
        /// Registers type of TType.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="includeAttribute"></param>
        /// <param name="excludeAttribute"></param>
        public void RegisterType< TType >( Type includeAttribute = null, Type excludeAttribute = null )
        {
            RegisterType( typeof( TType ), includeAttribute );
        }

        /// <summary>
        /// Registers type of type parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeAttribute"></param>
        /// <param name="excludeAttribute"></param>
        /// <exception cref="ArgumentNullException">When type is null.</exception>
        public void RegisterType( Type type, Type includeAttribute, Type excludeAttribute = null )
        {
            if ( type == null ) throw new ArgumentNullException();

            // Key in dictionary is a list of attribute values.
            var propertyMap = new Dictionary< string[], string >();

            var propertyInfos = type.GetProperties( BindingFlags.Public | BindingFlags.SetProperty );

            foreach ( var propertyInfo in propertyInfos ) {

                if (excludeAttribute != null && propertyInfo.GetCustomAttributes( excludeAttribute, true ).Any()) {
                    continue;
                }

                List< string > attributeValues = includeAttribute != null 
                                               ? propertyInfo.GetCustomAttributes( includeAttribute, true )
                                                             .Select( a => a.ToString().RemoveWhitespaces().ToUpperInvariant() )
                                                             .ToList() 
                                               : new List< string >();

                attributeValues.Add( propertyInfo.Name.ToUpperInvariant() );

                propertyMap[ attributeValues.ToArray() ] = propertyInfo.Name;
            }

            _typeDictionary[ type ] = propertyMap;
        }

        /// <summary>
        /// Returns tuple of Type and Dictionary&lt; propertyName, header &gt;
        /// </summary>
        /// <param name="sheetTable"><see cref="SheetTable"/></param>
        /// <returns>Tuple of Type and Dictionary&lt; propertyName, header &gt;</returns>
        public (Type type, Dictionary< string, (string header, int column) > propertyMap) GetTypeWithMap( SheetTable sheetTable )
        {
            var headerMap = sheetTable.HeaderMap.ToArray();
            var propertyMap = new Dictionary< string, (string header, int column) >();

            foreach ( var type in _typeDictionary.Keys ) {

                var propertyAttributes = _typeDictionary[ type ];
                bool found = false;

                foreach ( var header in headerMap.ToArray() ) {

                    foreach ( var propertyIdentity in propertyAttributes.Keys.OrderBy( a => a.Length ) ) {

                        if ( propertyIdentity.Contains( header.header ) ) {

                            found = true;
                            propertyMap[ propertyAttributes[ propertyIdentity ] ] = header;
                            propertyAttributes.Remove( propertyIdentity );
                            break;
                        }
                    }

                    if (!found) break;
                }

                if ( found ) return (type, propertyMap);
            }

            return (null, null);
        }
    }
}
