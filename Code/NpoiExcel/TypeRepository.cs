using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            RegisterType( typeof( TType ), includeAttribute, excludeAttribute );
        }

        public IEnumerable< Type > GetRegistredTypes () => _typeDictionary.Keys;

        public IEnumerable< string > GetPropertyNames ( Type type )
        {
            if ( type == null ) return new string[0];

            if (!_typeDictionary.TryGetValue( type, out var propertiesMap )) return new string[0];

            return propertiesMap.Values;
        }

        /// <summary>
        /// Registers type of type parameter. If includeAttribute is setted then value of ToString() method of this
        /// attribute will be considered in type searching by SheetTable.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeAttribute"></param>
        /// <param name="excludeAttribute"></param>
        /// <exception cref="ArgumentNullException">When type is null.</exception>
        public virtual void RegisterType( Type type, Type includeAttribute = null, Type excludeAttribute = null )
        {
            if ( type == null ) throw new ArgumentNullException();

            // Key in dictionary is a list of attribute values.
            var propertyMap = new Dictionary< string[], string >();

            var propertyInfos = type.GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetProperty )
                                    .Where( pi => pi.GetSetMethod( false ) != null );

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
        public virtual (Type type, Dictionary< string, (string header, int column) > propertyMap) GetTypeWithMap( SheetTable sheetTable )
        {
            var headerMapArray = sheetTable.HeaderMapSet.ToArray();

            foreach ( var type in _typeDictionary.OrderByDescending( t => t.Value.Count ).Select( t => t.Key ) ) {

                var propertyToSheetMap = new Dictionary< string, (string header, int column) >();
                var propertyNamesMap = _typeDictionary[ type ];
                if ( propertyNamesMap.Count > headerMapArray.Length ) continue;

                var iPropertyNamesMap = _typeDictionary[ type ].Keys.ToList();

                foreach ( var headerMap in headerMapArray.ToArray() ) {

                    var checkedHeader = headerMap.header.RemoveWhitespaces().ToUpperInvariant();

                    foreach ( var propertyIdentity in iPropertyNamesMap.OrderBy( a => a.Length ) ) {

                        if ( propertyIdentity.Any( p => p.Equals( checkedHeader ) ) ) {

                            propertyToSheetMap[ propertyNamesMap[ propertyIdentity ] ] = headerMap;
                            iPropertyNamesMap.Remove( propertyIdentity );
                            break;
                        }
                    }
                }

                if ( 0 == iPropertyNamesMap.Count ) return (type, propertyToSheetMap);
            }

            return (null, null);
        }
    }
}
