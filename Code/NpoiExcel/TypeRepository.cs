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
using NpoiExcel.Attributes;

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
        public void RegisterType( Type type, Type includeAttribute = null, Type excludeAttribute = null )
        {
            if ( type == null ) throw new ArgumentNullException();

            _typeDictionary[ type ] = GetPropertyMap( type, includeAttribute, excludeAttribute );
        }

        public static Dictionary< string[], string > GetPropertyMap ( Type type,
                                                                      Type includeAttribute = null, 
                                                                      Type excludeAttribute = null )
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

            return propertyMap;
        }

        /// <summary>
        /// Returns tuple of Type and Dictionary&lt; propertyName, header &gt;
        /// </summary>
        /// <param name="sheetTable"><see cref="SheetTable"/></param>
        /// <returns>Tuple of Type and Dictionary&lt; propertyName, header &gt;</returns>
        public (Type type, Dictionary< string, (string header, int column) > propertyMap) GetTypeWithMap ( SheetTable sheetTable )
        {
            var sheetHeaderMap = sheetTable.SheetHeaderMap.ToArray();

            foreach ( var type in _typeDictionary.OrderByDescending( t => t.Value.Count ).Select( t => t.Key ) ) {

                // check for speed
                var propertyNamesMap = _typeDictionary[ type ];
                if ( propertyNamesMap.Count > sheetHeaderMap.Length ) continue;

                // successfull token
                if ( TryGetPropertyMap( sheetHeaderMap, propertyNamesMap, out var propertyToSheetMap ) ) return (type, propertyToSheetMap);
            }

            return (null, null);
        }

        public static Dictionary< string, (string header, int column) > GetEmptyPropertyMap () => new Dictionary< string, (string header, int column) >();

        /// <summary>
        /// Tries to get property map.
        /// </summary>
        /// <param name="sheetTable"></param>
        /// <param name="type"></param>
        /// <param name="propertyMap"></param>
        /// <returns></returns>
        public static bool TryGetPropertyMap ( SheetTable sheetTable, Type type, out Dictionary< string, (string header, int column) > propertyMap )
        {
            if ( type == null ) { throw new ArgumentNullException( nameof( type ), "Type cannot be null." ); }

            var propertyNamesMap = GetPropertyMap( 

                type, 
                includeAttribute: typeof( HeaderAttribute), 
                excludeAttribute: typeof( HiddenAttribute ) 
            );

            var sheetHeaderMap = sheetTable.SheetHeaderMap.ToArray();

            return TryGetPropertyMap( sheetHeaderMap, propertyNamesMap, out propertyMap );
        }

        private static bool TryGetPropertyMap ( (string header, int column)[] sheetHeaderMap, Dictionary< string[], string > propertyNamesMap, out Dictionary< string, (string header, int column) > propertyMap )
        {
            var propertyToSheetMap = GetEmptyPropertyMap();
            var iPropertyNamesMap = propertyNamesMap.Keys.ToList();

            foreach ( var headerMap in sheetHeaderMap) {

                var checkedHeader = headerMap.header.RemoveWhitespaces().ToUpperInvariant();

                foreach ( var propertyIdentity in iPropertyNamesMap.OrderBy( a => a.Length ) ) {

                    if ( propertyIdentity.Any( p => p.Equals( checkedHeader ) ) ) {

                        propertyToSheetMap[ propertyNamesMap[ propertyIdentity ] ] = headerMap;
                        iPropertyNamesMap.Remove( propertyIdentity );
                        break;
                    }
                }
            }

            if ( 0 == iPropertyNamesMap.Count ) {
                propertyMap = propertyToSheetMap;
                return true;
            }

            propertyMap = new Dictionary< string, (string header, int column) >();
            return false;
        }
    }
}
