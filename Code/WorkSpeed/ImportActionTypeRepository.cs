using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using NpoiExcel;
using WorkSpeed.Interfaces;

namespace WorkSpeed
{
    public class ImportActionTypeRepository : ITypeRepository
    {
        private readonly Dictionary< Type, Dictionary< string[], string > > _typeDictionary = new Dictionary< Type, Dictionary< string[], string > >();

        public void RegisterType< TType >( Type propertyAttribute = null )
        {
            // Key in dictionary is a list of attribute values.
            var propertyMap = new Dictionary< string[], string >();

            var propertyInfos = typeof( TType ).GetProperties( BindingFlags.Public | BindingFlags.SetProperty );

            foreach ( var propertyInfo in propertyInfos ) {

                List< string > attributeValues = propertyAttribute != null 
                                               ? propertyInfo.GetCustomAttributes( propertyAttribute, true )
                                                             .Select( a => a.ToString().RemoveWhitespaces().ToUpperInvariant() )
                                                             .ToList() 
                                               : new List< string >();

                attributeValues.Add( propertyInfo.Name.ToUpperInvariant() );

                propertyMap[ attributeValues.ToArray() ] = propertyInfo.Name;
            }

            _typeDictionary[ typeof( TType ) ] = propertyMap;
        }

        /// <summary>
        /// Returns tuple of Type and Dictionary&lt; propertyName, header &gt;
        /// </summary>
        /// <param name="sheetTable"></param>
        /// <returns></returns>
        public (Type type, Dictionary< string, string > map) GetTypeWithMap( SheetTable sheetTable )
        {
            var fileHeaders = sheetTable.Headers.ToList();
            var propertyMap = new Dictionary< string, string >();

            foreach ( var type in _typeDictionary.Keys ) {

                var propertyAttributes = _typeDictionary[ type ];
                bool found = false;

                foreach ( var fileHeader in fileHeaders.ToArray() ) {

                    foreach ( var propertyIdentity in propertyAttributes.Keys.OrderBy( a => a.Length ) ) {

                        if ( propertyIdentity.Contains( fileHeader ) ) {

                            found = true;
                            propertyMap[ propertyAttributes[ propertyIdentity ] ] = fileHeader;
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
