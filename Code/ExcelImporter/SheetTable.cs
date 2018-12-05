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

            if ((StartCell = FindStartCell (Sheet)) < CellPoint.ZeroPoint) throw new ArgumentException("Sheet has no data", nameof(sheet));
            EndCell = FindEndCell (Sheet);
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
        /// Returns normalized header by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this [int index]
        {
            get {
                if (!_normalizedHeaders.ContainsKey (index)) throw new IndexOutOfRangeException();
                return _normalizedHeaders[index];
            }
        }

        private static CellPoint FindStartCell (ISheet sheet)
        {
            int row = sheet.FirstRowNum;
            int column = sheet.GetRow (row)?.FirstCellNum ?? -1;
            if (-1 == column) return CellPoint.NegativePoint;

            while (sheet.GetRow (row)?.GetCell (column)?.CellType == CellType.Blank) {

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

            while (sheet.GetRow (row)?.GetCell (column)?.CellType == CellType.Blank) {

                ++column;

                if (column >= sheet.GetRow (row)?.LastCellNum) {

                    ++row;
                    column = startColumn;
                }
            }

            return new CellPoint(column, row);
        }

        private static CellPoint FindEndCell(ISheet sheet)
        {
            int row = sheet.LastRowNum;
            int column = sheet.GetRow(row).LastCellNum - 1;

            while (sheet.GetRow(row)?.GetCell(column)?.CellType == CellType.Blank) {

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
                return new CellPoint(column + 1, row + 1);
            }

            var lastColumn = column;
            row = sheet.LastRowNum;

            while (sheet.GetRow (row)?.GetCell (column)?.CellType == CellType.Blank) {

                --column;

                if (column < sheet.GetRow (row)?.FirstCellNum) {

                    --row;
                    column = lastColumn;
                }
            }

            return new CellPoint(column + 1, row + 1);
        }

        private static Dictionary<int, string> GetHeaders (ISheet sheet, CellPoint startPoint, CellPoint endPoint)
        {
            var headers = new Dictionary<int, string> (endPoint.Column - startPoint.Column);

            for (int i = startPoint.Column; i < endPoint.Column; ++i) {

                headers[i] = sheet.GetRow (startPoint.Row).GetCell (i)?.StringCellValue ?? "";
            }

            return headers;
        }

        private static Dictionary<int, string> GetNormalizedHeaders(Dictionary<int, string> headers, CellPoint startPoint, CellPoint endPoint)
        {
            var normalizedHeaders = new Dictionary<int, string>(endPoint.Column - startPoint.Column);

            for (int i = startPoint.Column; i < endPoint.Column; ++i) {

                headers[i] = headers[i].RemoveWhitespaces().ToUpperInvariant();
            }

            return normalizedHeaders;
        }
        #endregion
    }
}
