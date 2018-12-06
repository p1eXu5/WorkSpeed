using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using WorkSpeed.Import;

namespace ExcelImporter
{
    public struct SheetTable
    {
        #region Fileds

        public readonly ISheet Sheet;
        public readonly CellPoint StartCell;
        public readonly CellPoint EndCell;
        public readonly int Lenght;

        private readonly Dictionary<int, string> _normalizedHeaders;
        private readonly Dictionary<int, string> _headers;
        
        #endregion


        #region Constructor

        /// <summary>
        /// Creates instance of SheetTable.
        /// </summary>
        /// <param name="sheet"></param>
        public SheetTable(ISheet sheet)
        {
            Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet), "Sheet can't be null!");

            (short minColumn, short maxColumn) boundColumns;

            try {
                boundColumns = GetBoundColumns (Sheet);
            }
            catch (NullReferenceException) {
                throw new ArgumentException("Sheet has no data", nameof(sheet));
            }

            if ((StartCell = FindStartCell (Sheet, boundColumns.minColumn)) < CellPoint.ZeroPoint) throw new ArgumentException("Sheet has no data", nameof(sheet));
            EndCell = FindEndCell (Sheet, boundColumns.maxColumn);

            Lenght = EndCell.Row - StartCell.Row;

            _headers = GetHeaders (sheet, StartCell, EndCell);
            _normalizedHeaders = GetNormalizedHeaders (_headers, StartCell, EndCell);
        }

        #endregion


        #region Properties

        public IEnumerable<string> Headers => _headers.Values;
        public IEnumerable<string> NormalizedHeaders => _normalizedHeaders.Values;
        
        #endregion


        #region Methods

        /// <summary>
        /// Returns normalized header by column.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string this [int column]
        {
            get {
                if (!_normalizedHeaders.ContainsKey (column)) throw new IndexOutOfRangeException("Column was outside the bounds of sheet table!");
                return _normalizedHeaders[column];
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

        private static Dictionary<int, string> GetHeaders (ISheet sheet, CellPoint startPoint, CellPoint endPoint)
        {
            var headers = new Dictionary<int, string> (endPoint.Column - startPoint.Column);

            for (int i = startPoint.Column; i < endPoint.Column; ++i) {

                headers[i] = sheet.GetRow (startPoint.Row).GetCell (i)?.StringCellValue ?? "";
            }

            return headers;
        }

        private static Dictionary<int, string> GetNormalizedHeaders(Dictionary<int, string> headers, CellPoint startCell, CellPoint endCell)
        {
            var normalizedHeaders = new Dictionary<int, string>(endCell.Column - startCell.Column);

            for (int i = startCell.Column; i < endCell.Column; ++i) {

                normalizedHeaders[i] = headers[i].RemoveWhitespaces().ToUpperInvariant();
            }

            return normalizedHeaders;
        }
        #endregion
    }
}
