using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using NpoiExcel.Attributes;
using NpoiExcel.Tests.Factory;
using NPOI.SS.UserModel;
using NUnit.Framework;
// ReSharper disable RedundantBoolCompare
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace NpoiExcel.Tests.UnitTests
{
    [ TestFixture ]
    public class TypeRepositoryUnitTests
    {
        [ SetUp ]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }



        [ Test ]
        public void RegisterType_TypeIsNull_Throws ()
        {
            // Arrange:
            var typeRepository = GetTypeRepository();

            // Action:
            var ex = Assert.Catch<ArgumentNullException>( () => typeRepository.RegisterType( null ) );

            // Assert:
            StringAssert.Contains( "", ex.Message );
        }

        [ Test ]
        public void RegisterType_TypeNotNull_AddsTypeToDictionary ()
        {
            // Arrange:
            var typeRepository = GetTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( TestType3 ) );

            // Assert:
            Assert.That( typeRepository.GetRegistredTypes().Any() );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_ElseParametersByDefault__AddsAllPublicPropertiesWithPublicSetter ()
        {
            // Arrange:
            var typeRepository = GetTypeRepository();
            typeRepository.RegisterType( typeof( TestType3 ) );

            // Action:
            var registredProperties = typeRepository.GetPropertyNames( typeof( TestType3 ) );

            // Assert:

            Assert.That( registredProperties, Is.EquivalentTo( TestType3.WithHiddenPropertyNames ) );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_IncludAttribute__AddsPublicPropertiesWithPublicSetter ()
        {
            // Arrange:
            var typeRepository = GetTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( TestType3 ), typeof( HeaderAttribute ) );
            var propertiNames = typeRepository.GetPropertyNames( typeof( TestType3 ) );

            // Assert:

            Assert.That( propertiNames, Is.EquivalentTo( TestType3.WithHiddenPropertyNames ) );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_ExcludAttribute__AddsPublicPropertiesWithPublicSetterExceptPropertiesWithExcludeAttribute ()
        {
            // Arrange:
            var typeRepository = GetTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( TestType3 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            var propertiNames = typeRepository.GetPropertyNames( typeof( TestType3 ) );

            // Assert:

            Assert.That( propertiNames, Is.EquivalentTo( TestType3.HiddenlessPropertyNames ) );
        }



        [ Test ]
        public void GetPropertyMap__TypeNotNull_IncludeAndExcludeAttributesAreSetted__RegistersAttributesWithPropertyNames ()
        {
            // Arrange:
            var type = typeof( TestType );

            // Action:
            var propertyMap = TypeRepository.GetPropertyMap( type, typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            // Assert:
            Assert.That( propertyMap.Keys, Is.EquivalentTo( TestType.PropertyAttributesCollection ) );
        }



        [ Test ]
        public void GetTypeWithMap_HasNoTypesInTypeReposytory_ReturnsTupleWithNulls ()
        {
            // Arrange:
            var sheetTable = GetMockedSheetTable( TestType.TableData );
            var typeRepository = GetTypeRepository();

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( resTuple.Equals( (null, null) ) );
        }

        [ Test ]
        public void GetTypeWithMap_HasNoTypeCorrespondedSheetTableHeaders_ReturnsTupleWithNulls ()
        {
            // Arrange:
            string[,] table = new string[,] { { "Name" }, { "SomeName" } };
            var sheetTable = GetMockedSheetTable( table );
            var typeRepository = GetTypeRepository();
            typeRepository.RegisterType( typeof( TestType ) );

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( resTuple.Equals( (null, null) ) );
        }

        [ Test ]
        public void GetTypeWithMap_HasTypeCorrespondedSheetTableHeaders_ReturnsTuple ()
        {
            // Arrange:
            var testType = typeof( TestType );
            var sheetTable = GetMockedSheetTable( TestType.TableData );
            var typeRepository = GetTypeRepository();
            typeRepository.RegisterType( testType, typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( testType == resTuple.type, $"Returned type is { resTuple.type?.Name ?? "null" }\n" );
        }

        [ Test ]
        public void GetTypeWithMap_AllTypesCorrespondsSheetTableHeaders_ReturnsMoreFullCorrespondedType ()
        {
            // Arrange:
            var repository = GetTypeRepository();
            repository.RegisterType( typeof( TestType ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repository.RegisterType( typeof( TestType2 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repository.RegisterType( typeof( TestType3 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            var sheetTable = GetMockedSheetTable( TestType.TableData );

            // Action:
            var typeMap = repository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( typeMap.type.IsAssignableFrom( typeof( TestType ) ), $"It was {typeMap.type.Name}" );
        }

        [ Test ]
        public void GetTypeWithMap_SheetHasLessColumnsThanTypeHasProperties_ReturnsTupleWithNulls ()
        {
            // Arrange:
            var sheetTable = GetMockedSheetTable( TestType.TableData );
            var repository = GetTypeRepository();
            repository.RegisterType( typeof( TestType4 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            // Action:
            var typeMap = repository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( typeMap.Equals( (null, null) ) );
        }



        [ Test ]
        public void TryGetPropertyMap_TypeValid_ReturnsPropertyMap ()
        {
            var type = typeof( TestType );
            var propertyMap = TypeRepository.GetPropertyMap( type );
            var sheet = MockedSheetFactory.GetMockedSheet( TestType.TableData );
            var sheetTable = new SheetTable( sheet );

            var res = TypeRepository.TryGetPropertyMap( sheetTable, type, out var map );

            Assert.That( res );
            Assert.That( propertyMap.Keys.All( k => !String.IsNullOrWhiteSpace( k[0] ) ));
        }



        #region Factory

        private TypeRepository GetTypeRepository ()
        {
            return new TypeRepository();
        }

        private Type GetTestType () => typeof( TestType ); 

        private SheetTable GetMockedSheetTable ( string[,] tableData )
        {
            return new SheetTable( MockedSheetFactory.GetMockedSheet( tableData ) );
        }

        class TestType
        {
            public static readonly string[,] TableData = {
                { "Имя клиента", "Address", "Какой-то столбец", "Почтовый Индекс " },
                {        "Вася", "Коробка",                 "",           "134567" }
            };

            public static readonly List< string[] > PropertyAttributesCollection = new List< string[] > {
                new[] { "ИМЯ", "ИМЯКЛИЕНТА", "NAME" },
                new[] { "ADDRESS" },
                new[] { "ПОЧТОВЫЙИНДЕКС", "POSTCODE" },
            };

            [ Header( "Имя" ) ]
            [ Header( "Имя клиента" ) ]
            public string Name { get; set; }

            public string Address { get; set; }

            [ Header( "Почтовый индекс " ) ]
            public int PostCode { get; set; }

            [ Hidden ]
            public Type SomeType { get; set; }
        }

        class TestType2
        {
            public static readonly string[,] TableData = {
                { "Имя клиента", "Address", "Какой-то столбец", "Почтовый Индекс ", "№ чего-то там" },
                {        "Вася", "Коробка",                 "",           "134567",     "sdfdsf-sd" }
            };

            [ Header( "Имя" ) ]
            [ Header( "Имя клиента" ) ]
            public string Name { get; set; }

            public string Address { get; set; }

            [ Header( "Почтовый индекс " ) ]
            public int PostCode { get; set; }

            [ Hidden ]
            public Type SomeType { get; set; }

            [ Header( "№ чего-то там" ) ]
            public string SomeProperty { get; set; }
        }

        class TestType3
        {
            public static readonly string[] HiddenlessPropertyNames = new[] {

                nameof( Name ),
                nameof( Address ),
                nameof( PostCode ),
                nameof( SomeProperty ),
                nameof( ElseOneSomeProperty )
            };

            public static readonly string[] WithHiddenPropertyNames = new[] {

                nameof( Name ),
                nameof( Address ),
                nameof( PostCode ),
                nameof( SomeType ),
                nameof( SomeProperty ),
                nameof( ElseOneSomeProperty )
            };

            [ Header( "Имя" ) ]
            [ Header( "Имя клиента" ) ]
            public string Name { get; set; }

            public string Address { get; set; }

            [ Header( "Почтовый индекс " ) ]
            public int PostCode { get; set; }

            [ Hidden ]
            public Type SomeType { get; set; }

            [ Header( "№ чего-то там" ) ]
            public string SomeProperty { get; set; }

            [ Header( "Какой-то столбец" ) ]
            public string ElseOneSomeProperty { get; set; }

            public string PrivateSetProperty { get; private set; }
        }

        class TestType4
        {
            [ Header( "Имя" ) ]
            [ Header( "Имя клиента" ) ]
            public string Name { get; set; }

            public string Address { get; set; }

            [ Header( "Почтовый индекс " ) ]
            public int PostCode { get; set; }

            [ Hidden ]
            public Type SomeType { get; set; }

            [ Header( "№ чего-то там" ) ]
            public string SomeProperty { get; set; }

            [ Header( "Какой-то столбец" ) ]
            public string ElseOneSomeProperty { get; set; }

            public int ExcessProperty { get; set; }
        }

        #endregion
    }
}
