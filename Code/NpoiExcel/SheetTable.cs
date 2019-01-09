using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using NPOI.SS.UserModel;

namespace NpoiExcel
{
    /// <summary>
    /// Represents data table contained in an Excel sheet what had been mported from an Excel file.
    /// </summary>
    public struct SheetTable
    {
        #region Fileds

        public readonly int RowCount;
        public readonly int ColumnCount;

        private readonly ISheet _sheet;
        private readonly CellPoint _startCell;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly CellPoint _endCell;

        private readonly HashSet<( string header, int column )> _sheetHeaderMap;
        
        #endregion



        #region Constructor

        /// <summary>
        /// Creates instance of SheetTable.
        /// </summary>
        /// <param name="sheet"></param>
        public SheetTable(ISheet sheet)
        {
            _sheet = sheet ?? throw new ArgumentNullException(nameof(sheet), "sheet can't be null!");

            (short minColumn, short maxColumn, int lastRow) bounds;

            try {
                bounds = GetBound (_sheet);
            }
            catch (NullReferenceException) {
                throw new ArgumentException("sheet has no data", nameof(sheet));
            }

            if ( (_startCell = FindStartCell( _sheet, bounds.minColumn, bounds.lastRow ) ) < CellPoint.ZeroPoint) throw new ArgumentException("_sheet has no data", nameof(sheet));
            _endCell = FindEndCell( _sheet, bounds.maxColumn, bounds.lastRow );

            // Row #0 is headers not counted
            RowCount = _endCell.Row - _startCell.Row - 1;
            ColumnCount = _endCell.Column - _startCell.Column;

            _sheetHeaderMap = GetHeaderMap (sheet, _startCell, _endCell);
        }

        #endregion



        #region Properties

        /// <summary>
        /// Unformated excel table headers.
        /// </summary>
        public IEnumerable<string> Headers => _sheetHeaderMap.Select( h => h.header );
        public IEnumerable<( string header, int column )> SheetHeaderMap => _sheetHeaderMap;
        
        #endregion



        #region Methods

        public CellValue this [int row, int column]
        {
            get {

                if ((row | RowCount) == 0) throw new InvalidOperationException("Sheet has only headers.");
                if (row < 0 || row >= RowCount) throw new IndexOutOfRangeException("Row was outside the bounds of sheet table.");
                if (column < 0 || column >= ColumnCount) throw new IndexOutOfRangeException("Column was outside the bounds of sheet table.");

                var cv = new CellValue (_sheet.GetRow (_startCell.Row + row + 1)?.GetCell (_startCell.Column + column));

                //Debug.WriteLine( $"CellValue this [int row, int column] - {sw.Elapsed}" ); // 0000798

                return cv;
            }
        }

        private static (short minColumn, short maxColumn, int lastRow) GetBound(ISheet sheet)
        {
            var minColumn = sheet.GetRow (sheet.FirstRowNum).FirstCellNum;
            short maxColumn;
            int lastRowCounter = sheet.LastRowNum;
            int lastRow = -1;

            do {
                maxColumn = sheet.GetRow( lastRowCounter-- )?.LastCellNum ?? -1;
                if ( lastRow < 0 && maxColumn >= 0 ) lastRow = lastRowCounter + 1;

            } while ( (maxColumn < 0 && lastRowCounter > 0) || maxColumn < minColumn );

            if ( maxColumn < minColumn ) throw new Exception( " maxColumn < minColumn " );

            for (int i = sheet.FirstRowNum; i <= lastRow; i++) {

                var row = sheet.GetRow (i);
                if (null == row) continue;

                if (maxColumn < row.LastCellNum) {
                    maxColumn = row.LastCellNum;
                }

                if (minColumn > row.FirstCellNum) {
                    minColumn = row.FirstCellNum;
                }
            }

            return (minColumn, maxColumn, lastRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="minColumn"></param>
        /// <param name="lastRaw"></param>
        /// <returns></returns>
        private static CellPoint FindStartCell ( ISheet sheet, short minColumn, int lastRaw )
        {
            int row = sheet.FirstRowNum;
            short column = minColumn;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow (row).GetCell (column).CellType == CellType.Blank) {

                ++row;

                if (row > lastRaw) {

                    row = sheet.FirstRowNum;
                    ++column;

                    if (column >= sheet.GetRow (row).LastCellNum) {

                        return CellPoint.NegativePoint;
                    }
                }
            }

            if (row == sheet.FirstRowNum) {
                return new CellPoint(column, row);
            }

            var startColumn = column;
            row = sheet.FirstRowNum;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow(row).GetCell(column).CellType == CellType.Blank) {

                ++column;

                if (column >= sheet.GetRow (row)?.LastCellNum) {

                    ++row;
                    column = startColumn;
                }
            }

            return new CellPoint(startColumn, row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="maxColumn"></param>
        /// <returns></returns>
        private static CellPoint FindEndCell(ISheet sheet, short maxColumn, int lastRow )
        {
            int row = lastRow;
            short column = maxColumn;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow(row).GetCell(column).CellType == CellType.Blank) {

                --row;

                if (row < sheet.FirstRowNum) {

                    row = lastRow;
                    --column;

                    if (column < sheet.GetRow(row).FirstCellNum) {

                        return CellPoint.NegativePoint;
                    }
                }
            }
            
            if (row == lastRow) {
                return new CellPoint((short)(column + 1), row + 1);
            }

            var lastColumn = column;
            row = lastRow;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow(row).GetCell(column).CellType == CellType.Blank) {

                --column;

                if (column < sheet.GetRow (row)?.FirstCellNum) {

                    --row;
                    column = lastColumn;
                }
            }

            return new CellPoint((short)(lastColumn + 1), row + 1);
        }

        private static HashSet<( string, int )> GetHeaderMap (ISheet sheet, CellPoint startPoint, CellPoint endPoint)
        {
            var headerSet = new HashSet<( string header, int column )> ();

            for (int i = startPoint.Column; i < endPoint.Column; ++i) {

                string header = "";

                switch ( sheet.GetRow( startPoint.Row ).GetCell( i )?.CellType ) {

                    case CellType.String:
                        header = sheet.GetRow( startPoint.Row ).GetCell( i ).StringCellValue ?? "";
                        break;

                    case CellType.Numeric:
                        header = sheet.GetRow( startPoint.Row ).GetCell( i ).NumericCellValue.ToString(CultureInfo.CurrentCulture);
                        break;
                }

                headerSet.Add( ( header, i - startPoint.Column ) );
            }

            return headerSet;
        }

        // ReSharper disable once UnusedMember.Local
        private int[] GetColumnsByHeader( string header )
        {
            if ( null == header ) throw new ArgumentNullException();

            return _sheetHeaderMap.Where( h => h.header.Equals( header ) ).Select( h => h.column ).ToArray();
        }


        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals( null, obj ) ) return false;
            return obj is SheetTable other && Equals( other );
        }

        public override int GetHashCode ()
        {
            return _sheet != null ? _sheet.GetHashCode() : 0;
        }

        public bool Equals ( SheetTable other )
        {
            return ReferenceEquals( _sheet, other._sheet ) || _sheet.Equals( other._sheet );
        }

        public static bool operator == ( SheetTable sheetTableA, SheetTable sheetTableB )
        {
            return sheetTableA.Equals( sheetTableB );
        }

        public static bool operator != ( SheetTable sheetTableA, SheetTable sheetTableB )
        {
            return !sheetTableA.Equals( sheetTableB );
        }

        #endregion
    }
}
