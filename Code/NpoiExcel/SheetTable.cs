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

        private readonly HashSet<( string header, int column )> _headerMapSet;
        
        #endregion


        #region Constructor

        /// <summary>
        /// Creates instance of SheetTable.
        /// </summary>
        /// <param name="sheet"></param>
        public SheetTable(ISheet sheet)
        {
            _sheet = sheet ?? throw new ArgumentNullException(nameof(sheet), "sheet can't be null!");

            (short minColumn, short maxColumn) boundColumns;

            try {
                boundColumns = GetBoundColumns (_sheet);
            }
            catch (NullReferenceException) {
                throw new ArgumentException("sheet has no data", nameof(sheet));
            }

            if ((_startCell = FindStartCell (_sheet, boundColumns.minColumn)) < CellPoint.ZeroPoint) throw new ArgumentException("_sheet has no data", nameof(sheet));
            _endCell = FindEndCell (_sheet, boundColumns.maxColumn);

            // Row #0 is headers not counted
            RowCount = _endCell.Row - _startCell.Row - 1;
            ColumnCount = _endCell.Column - _startCell.Column;

            _headerMapSet = GetHeaderMap (sheet, _startCell, _endCell);
        }

        #endregion


        #region Properties

        public IEnumerable<string> Headers => _headerMapSet.Select( h => h.header );
        public IEnumerable<( string header, int column )> HeaderMapSet => _headerMapSet;
        
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

        private static (short minColumn, short maxColumn) GetBoundColumns(ISheet sheet)
        {
            var minColumn = sheet.GetRow (sheet.FirstRowNum).FirstCellNum;
            var maxColumn = sheet.GetRow (sheet.LastRowNum).LastCellNum;

            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++) {

                var row = sheet.GetRow (i);
                if (null == row) continue;

                if (maxColumn < row.LastCellNum) {
                    maxColumn = row.LastCellNum;
                }

                if (minColumn > row.FirstCellNum) {
                    minColumn = row.FirstCellNum;
                }
            }

            return (minColumn, maxColumn);
        }

        private static CellPoint FindStartCell (ISheet sheet, short minColumn)
        {
            int row = sheet.FirstRowNum;
            short column = minColumn;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow (row).GetCell (column).CellType == CellType.Blank) {

                ++row;

                if (row > sheet.LastRowNum) {

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

        private static CellPoint FindEndCell(ISheet sheet, short maxColumn)
        {
            int row = sheet.LastRowNum;
            short column = maxColumn;

            while (sheet.GetRow(row)?.GetCell(column) == null
                   || sheet.GetRow(row).GetCell(column).CellType == CellType.Blank) {

                --row;

                if (row < sheet.FirstRowNum) {

                    row = sheet.LastRowNum;
                    --column;

                    if (column < sheet.GetRow(row).FirstCellNum) {

                        return CellPoint.NegativePoint;
                    }
                }
            }
            
            if (row == sheet.LastRowNum) {
                return new CellPoint((short)(column + 1), row + 1);
            }

            var lastColumn = column;
            row = sheet.LastRowNum;

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

            return _headerMapSet.Where( h => h.header.Equals( header ) ).Select( h => h.column ).ToArray();
        }

        #endregion
    }
}
