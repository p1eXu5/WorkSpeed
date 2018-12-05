using System;
using System.Collections.Generic;
using System.Linq;
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

            if ((StartCell = FindStartCell (Sheet)) < CellPoint.ZeroPoint) throw new ArgumentException("Sheet have not data", nameof(sheet));
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
            int y = sheet.FirstRowNum;
            int x = sheet.GetRow (y).FirstCellNum; 

            while (sheet.GetRow (y)?.GetCell (x)?.CellType == CellType.Blank) {

                ++y;

                if (y > sheet.LastRowNum) {

                    y = sheet.FirstRowNum;
                    ++x;

                    if (x >= sheet.GetRow (y).LastCellNum) {

                        return CellPoint.NegativePoint;
                    }
                }
            }

            return new CellPoint(x, y);
        }

        private static CellPoint FindEndCell(ISheet sheet)
        {
            int y = sheet.LastRowNum;
            int x = sheet.GetRow(y).LastCellNum - 1;

            while (sheet.GetRow(y)?.GetCell(x)?.CellType == CellType.Blank) {

                --y;

                if (y < sheet.FirstRowNum) {

                    y = sheet.LastRowNum;
                    --x;

                    if (x < sheet.GetRow(y).FirstCellNum) {

                        return CellPoint.NegativePoint;
                    }
                }
            }

            return new CellPoint(x + 1, y + 1);
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
