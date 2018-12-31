using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;

namespace NpoiExcel.Tests.Factory
{
    public class MockedSheetFactory
    {
        public static ISheet EmptySheet => new XSSFSheet();

        /// <summary>
        /// Return test cases.
        /// </summary>
        /// <param name="firstColumn"></param>
        /// <param name="firsrtRow"></param>
        /// <returns></returns>
        public static IEnumerable HeaderTestCases (int firstColumn, int firsrtRow)
        {
            // 1
            var area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 1, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #1 ({firstColumn}, {firsrtRow});");

            // 2
            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #2 ({firstColumn}, {firsrtRow});");

            // 3
            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 1, 1, 0, 0 },
                    { 1, 0, 1, 1 },
                    { 1, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #3 ({firstColumn}, {firsrtRow});");

            // 4
            area = new[,]
                {
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #4 ({firstColumn}, {firsrtRow});");

            // 5
            area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 1, 0, 1 },
                    { 1, 0, 1, 1 },
                    { 0, 0, 0, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #5 ({firstColumn}, {firsrtRow});");

            // 6
            area = new[,]
                {
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #6 ({firstColumn}, {firsrtRow});");

            // 7
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #7 ({firstColumn}, {firsrtRow});");

            // 8
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                    .SetName($"Test case #8 ({firstColumn}, {firsrtRow});");

            // 9
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .SetName($"Test case #9 ({firstColumn}, {firsrtRow});");
        }

        /// <summary>
        /// Return test cases.
        /// </summary>
        /// <param name="firstColumn"></param>
        /// <param name="firsrtRow"></param>
        /// <returns></returns>
        public static IEnumerable HeaderTestCasesWithReturns (int firstColumn, int firsrtRow)
        {
            // 1
            var area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 1, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns (new[]{"", "", "", "TESTSTRING" })
                                .SetName($"Test case #1 ({firstColumn}, {firsrtRow});");

            // 2
            area = new[,]
                {
                    { 1, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns(new[] { "TESTSTRING", "", "TESTSTRING", "" })
                                .SetName($"Test case #2 ({firstColumn}, {firsrtRow});");

            // 4
            area = new[,]
                {
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns(new[] { "TESTSTRING", "TESTSTRING", "TESTSTRING", "TESTSTRING" })
                                .SetName($"Test case #4 ({firstColumn}, {firsrtRow});");


            // 7
            area = new[,]
            {
                { 0, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 },
            };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                 .Returns(new[] { "TESTSTRING" })
                                 .SetName($"Test case #7 ({firstColumn}, {firsrtRow});");

            // 8
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                 .Returns(new[] { "TESTSTRING", "TESTSTRING" })
                                 .SetName($"Test case #8 ({firstColumn}, {firsrtRow});");

            // 9
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                 .Returns(new[] { "TESTSTRING" })
                                 .SetName($"Test case #9 ({firstColumn}, {firsrtRow});");
        }

        /// <summary>
        /// Return test cases.
        /// </summary>
        /// <param name="firstColumn"></param>
        /// <param name="firsrtRow"></param>
        /// <returns></returns>
        public static IEnumerable RowColumnCountsTestCases(int firstColumn, int firsrtRow)
        {
            // 1
            var area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                    { 1, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #1 ({firstColumn}, {firsrtRow});");

            // 2
            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #2 ({firstColumn}, {firsrtRow});");

            // 3
            area = new[,]
                {
                    { 1, 0, 0, 0 },
                    { 1, 1, 0, 0 },
                    { 1, 0, 1, 1 },
                    { 1, 0, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #3 ({firstColumn}, {firsrtRow});");

            // 4
            area = new[,]
                {
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 0, 0 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #4 ({firstColumn}, {firsrtRow});").SetCategory ("Headers");

            // 5
            area = new[,]
                {
                    { 0, 0, 0, 1 },
                    { 0, 1, 0, 1 },
                    { 1, 0, 1, 1 },
                    { 0, 0, 0, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #5 ({firstColumn}, {firsrtRow});");

            // 6
            area = new[,]
                {
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                };
            yield return new TestCaseData(GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((3, 4))
                                .SetName($"Test case #6 ({firstColumn}, {firsrtRow});");

            // 7
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData (GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((1, 1))
                                .SetName($"Test case #7 ({firstColumn}, {firsrtRow});");

            // 8
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData (GetMockedSheet((short)firstColumn, firsrtRow, area))
                                    .Returns((0, 2))
                                    .SetName($"Test case #8 ({firstColumn}, {firsrtRow});");

            // 9
            area = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                };
            yield return new TestCaseData (GetMockedSheet((short)firstColumn, firsrtRow, area))
                                .Returns((0, 1))
                                .SetName($"Test case #9 ({firstColumn}, {firsrtRow});");
        }

        public static ISheet GetMockedSheet ( short firstColumn, int firsrtRow, int[,] area )
        {
            var rowsMap = new Dictionary<int, HashSet<int>>();

            for ( int j = 0; j < area.GetLength( 0 ); j++ ) {
                for ( short i = 0; i < area.GetLength( 1 ); ++i ) {

                    if ( area[ j, i ] != 0 ) {

                        var rowNumber = j + firsrtRow;

                        if ( !rowsMap.Keys.Contains( rowNumber ) ) {
                            rowsMap[ rowNumber ] = new HashSet<int>();
                        }

                        rowsMap[ rowNumber ].Add( i + firstColumn );
                    }
                }
            }

            return GetSheetMock( rowsMap );
        }

        public static ISheet GetMockedSheet ( string[,] tableData )
        {
            if ( tableData == null ) throw new ArgumentNullException( nameof( tableData ), "Headers cannot be null." );
            if ( tableData.Length <= 0 ) return EmptySheet;

            var columnMap = new HashSet< int >( Enumerable.Range( 0, tableData.GetLength( 1 ) ) );

            var rowsMap = new Dictionary< int, HashSet< int > > ();

            for ( int i = 0; i < tableData.GetLength( 0 ); ++i ) {
                rowsMap[ i ] = columnMap;
            }

            return GetSheetMock( rowsMap, tableData );
        }

        private static ISheet GetSheetMock ( Dictionary< int, HashSet< int > > rowsMap, string[,] data )
        {
            // см. типы передаваемых параметров, а не возвращаемых!
            var sheetMock = new Mock< ISheet >();

            sheetMock.Setup( s => s.FirstRowNum ).Returns( rowsMap.Keys.Min() );
            sheetMock.Setup( s => s.LastRowNum ).Returns( rowsMap.Keys.Max() );

            sheetMock.Setup( s => s.GetRow( It.Is< int >( r => rowsMap.Keys.Contains( r ) ) ) )
                     .Returns( ( int r ) =>
                               {
                                   var rowMock = new Mock< IRow >();
                                   rowMock.Setup( s => s.FirstCellNum ).Returns( ( short )rowsMap[ r ].Min() );
                                   rowMock.Setup( s => s.LastCellNum ).Returns( ( short )(rowsMap[ r ].Max() + 1) );
                                   rowMock.Setup( row => row.GetCell( It.Is< int >( c => rowsMap[ r ].Contains( c ) ) ) )
                                          .Returns( ( int c ) => ReturnMockedStringCell( data, r, c ) );
                                   return rowMock.Object;
                               } );

            return sheetMock.Object;
        }

        private static ICell ReturnMockedStringCell ( string[,] data, int row, int column )
        {
            var stringCellMock = new Mock<ICell>();

            stringCellMock.Setup( c => c.CellType ).Returns( CellType.String );
            stringCellMock.Setup( c => c.StringCellValue ).Returns( data[ row, column ] );

            return stringCellMock.Object;
        }

        private static ISheet GetSheetMock ( Dictionary< int, HashSet< int > > rowsMap )
        {
            // см. типы передаваемых параметров, а не возвращаемых!
            var sheetMock = new Mock< ISheet >();

            sheetMock.Setup( s => s.FirstRowNum ).Returns( rowsMap.Keys.Min() );
            sheetMock.Setup( s => s.LastRowNum ).Returns( rowsMap.Keys.Max() );

            sheetMock.Setup( s => s.GetRow( It.Is< int >( r => rowsMap.Keys.Contains( r ) ) ) )
                     .Returns( ( int r ) =>
                               {
                                   var rowMock = new Mock< IRow >();
                                   rowMock.Setup( s => s.FirstCellNum ).Returns( ( short )rowsMap[ r ].Min() );
                                   rowMock.Setup( s => s.LastCellNum ).Returns( ( short )(rowsMap[ r ].Max() + 1) );
                                   rowMock.Setup( row => row.GetCell( It.Is< int >( c => rowsMap[ r ].Contains( c ) ) ) )
                                          .Returns( ReturnMockedStringCell() );
                                   return rowMock.Object;
                               } );

            return sheetMock.Object;
        }


        private static ICell ReturnMockedStringCell ()
        {
            var stringCellMock = new Mock<ICell>();

            stringCellMock.Setup( c => c.CellType ).Returns( CellType.String );
            stringCellMock.Setup( c => c.StringCellValue ).Returns( "Test string" );

            return stringCellMock.Object;
        }
    }
}
