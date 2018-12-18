using System;
using System.Globalization;
using System.Linq;
using Moq;
using NpoiExcel.Attributes;
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
        public void RegisterTypeGeneric_PassedAllParameters_CallsRegisterTypeWithAllParameters ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            // Action:
            typeRepository.RegisterType< FakeTypeReposytory >( typeof( HeaderAttribute), typeof( HiddenAttribute ) );

            // Assert:
            Assert.That( true == typeRepository.IsAllParamsPassed );
        }


        [ Test ]
        public void RegisterType_TypeIsNull_Throws ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            // Action:
            var ex = Assert.Catch<ArgumentNullException>( () => typeRepository.RegisterType( null ) );

            // Assert:
            StringAssert.Contains( "", ex.Message );
        }

        [ Test ]
        public void RegisterType_TypeNotNull_AddsTypeToDictionary ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( FakeTypeReposytory ) );

            // Assert:
            Assert.That( typeRepository.GetRegistredTypes().Any() );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_ElseParametersByDefault__AddsAllPublicPropertiesWithPublicSetter ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( FakeTypeReposytory ) );

            // Assert:

            Assert.That( 
                typeRepository.GetPropertyNames( typeof( FakeTypeReposytory ) )
                              .First()
                              .Equals( nameof( FakeTypeReposytory.PublicPropertyWithoutAttribute ) ) 
            );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_ElseParametersByDefault__DoNotAddPublicPropertiesWithPrivateSetter ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            // Action:
            typeRepository.RegisterType( typeof( FakeTypeReposytory ) );
            var propertiNames = typeRepository.GetPropertyNames( typeof( FakeTypeReposytory ) );

            // Assert:

            Assert.That( propertiNames, Has.No.Member( nameof( FakeTypeReposytory.IsAllParamsPassed ) ) );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_IncludAttribute__AddsPublicPropertiesWithPublicSetter ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            var expectedColl = new[] {

                nameof( FakeTypeReposytory.PublicPropertyWithoutAttribute ),
                nameof( FakeTypeReposytory.PublicPropertyWithIncludedAttribute ),
                nameof( FakeTypeReposytory.PublicPropertyWithExcludedAttribute ),
            };

            // Action:
            typeRepository.RegisterType( typeof( FakeTypeReposytory ), typeof( HeaderAttribute ) );
            var propertiNames = typeRepository.GetPropertyNames( typeof( FakeTypeReposytory ) );

            // Assert:

            Assert.That( propertiNames, Is.EquivalentTo( expectedColl ) );
        }

        [ Test ]
        public void RegisterType__TypeNotNull_ExcludAttribute__AddsPublicPropertiesWithPublicSetterExceptPropertiesWithExcludettribute ()
        {
            // Arrange:
            var typeRepository = GetFakeTypeRepository();

            var expectedColl = new[] {

                nameof( FakeTypeReposytory.PublicPropertyWithoutAttribute ),
                nameof( FakeTypeReposytory.PublicPropertyWithIncludedAttribute ),
            };

            // Action:
            typeRepository.RegisterType( typeof( FakeTypeReposytory ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            var propertiNames = typeRepository.GetPropertyNames( typeof( FakeTypeReposytory ) );

            // Assert:

            Assert.That( propertiNames, Is.EquivalentTo( expectedColl ) );
        }


        [ Test ]
        public void GetTypeWithMap_HasNoTypesInTypeReposytory_ReturnsTupleWithNulls ()
        {
            // Arrange:
            var sheetTable = GetMockedSheetTable();
            var typeRepository = GetFakeTypeRepository();

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( resTuple.Equals( (null, null) ) );
        }

        [ Test ]
        public void GetTypeWithMap_HasNoTypeCorrespondedSheetTableHeaders_ReturnsTupleWithNulls ()
        {
            // Arrange:
            var sheetTable = GetMockedSheetTable();
            var typeRepository = GetFakeTypeRepository();
            typeRepository.RegisterType( typeof( FakeTypeReposytory ) );

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( resTuple.Equals( (null, null) ) );
        }

        [ Test ]
        public void GetTypeWithMap_HasTypeCorrespondedSheetTableHeaders_ReturnsTuple ()
        {
            // Arrange:
            var testType = GetTestType();
            var sheetTable = GetMockedSheetTable();
            var typeRepository = GetFakeTypeRepository();
            typeRepository.RegisterType( testType, typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            // Action:
            var resTuple = typeRepository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( testType == resTuple.type );
        }

        [ Test ]
        public void GetTypeWithMap_AllTypesCorrespondsSheetTableHeaders_ReturnsMoreFullCorrespondedType ()
        {
            // Arrange:
            var repository = GetFakeTypeRepository();
            repository.RegisterType( typeof( TestType ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repository.RegisterType( typeof( TestType2 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repository.RegisterType( typeof( TestType3 ), typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            var sheetTable = GetMockedSheetTable();

            // Action:
            var typeMap = repository.GetTypeWithMap( sheetTable );

            // Assert:
            Assert.That( typeMap.type.IsAssignableFrom( typeof( TestType3 ) ), $"It was {typeMap.type.Name}" );
        }



        #region Factory

        private FakeTypeReposytory GetFakeTypeRepository ()
        {
            return new FakeTypeReposytory();
        }

        private Type GetTestType () => typeof( TestType ); 

        private SheetTable GetMockedSheetTable ()
        {
            string[,] sheetData = {
                { "Имя клиента", "Address", "Какой-то столбец", "Почтовый Индекс ", "№ чего-то там" },
                {        "Вася", "Коробка",                 "",           "134567",     "sdfdsf-sd" }
            };

            var mockISheet = new Mock< ISheet >();
            mockISheet.Setup( s => s.FirstRowNum ).Returns( 0 );
            mockISheet.Setup( s => s.LastRowNum ).Returns( 1 );

            mockISheet.Setup( s => s.GetRow( It.Is< int >( r => r >= 0 && r < sheetData.GetLength( 0 ) ) ) )
                      .Returns( ( int row ) =>
                                {
                                    var mockIRow = new Mock< IRow >();

                                    mockIRow.Setup( r => r.FirstCellNum ).Returns( ( short )0 );
                                    mockIRow.Setup( r => r.LastCellNum ).Returns( ( short )sheetData.GetLength( 1 ) );
                                    mockIRow
                                        .Setup( r => r.GetCell( It.Is< int >( c => c >= 0 && c <
                                                                                   sheetData.GetLength( 1 ) ) ) )
                                        .Returns( ( int cell ) =>
                                                  {
                                                      var mockICell = new Mock< ICell >();

                                                      mockICell.Setup( c => c.CellType ).Returns( CellType.String );
                                                      mockICell.Setup( c => c.StringCellValue )
                                                               .Returns( sheetData[ row, cell ] );

                                                      return mockICell.Object;
                                                  } );

                                    return mockIRow.Object;
                                } );

            return new SheetTable( mockISheet.Object );
        }

        class TestType
        {
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
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once MemberCanBePrivate.Local
        class FakeTypeReposytory : TypeRepository
        {
            public bool IsAllParamsPassed { get; private set; }
            public int PublicPropertyWithoutAttribute { get; set; }

            [ Header( "Included Attribute" ) ]
            public int PublicPropertyWithIncludedAttribute { get; set; }

            [ Hidden ]
            public int PublicPropertyWithExcludedAttribute { get; set; }

            public override void RegisterType ( Type type, Type includeAttribute = null, Type excludeAttribute = null )
            {
                if ( type != null && includeAttribute != null && excludeAttribute != null ) {
                    IsAllParamsPassed = true;
                }

                base.RegisterType( type, includeAttribute, excludeAttribute );
            }
        }

        #endregion
    }
}
