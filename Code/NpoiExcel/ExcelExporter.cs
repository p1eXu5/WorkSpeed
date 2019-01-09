using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NpoiExcel
{
    public static class ExcelExporter
    {
        public static void ExportData ( string fileName, IEnumerable< IExportingCell > exportingCells )
        {
            if ( fileName == null ) throw new ArgumentNullException( nameof( fileName ), "File name cannot be null." );
            if ( String.IsNullOrWhiteSpace( fileName ) ) throw new ArgumentException( @"File name cannot be empty.", nameof( fileName ) );
            
            using ( var stream = File.Create( fileName ) ) {

                ExportData( exportingCells, stream );
            }
        }

        public static void ExportData ( IEnumerable< IExportingCell > exportingCells, Stream stream, bool stayOpen = false )
        {
            if ( stream == null ) throw new ArgumentNullException( nameof( stream ), @"Stream cannot be null." );
            if ( exportingCells == null ) throw new ArgumentNullException( nameof( exportingCells ), @"Exporting cells cannot be null." );
            if ( !exportingCells.Any() ) throw new ArgumentException( @"exportingCells cannot be empty.", nameof( exportingCells ) );

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet( "0" );

            foreach ( var exportingCell in exportingCells ) {

                IRow row = sheet.GetRow( exportingCell.Row );

                if ( row == null ) {
                    row = sheet.CreateRow( exportingCell.Row );
                }

                ICell cell = row.GetCell( exportingCell.Column );

                if ( row.GetCell( exportingCell.Column ) == null ) {
                    cell = row.CreateCell( exportingCell.Column );
                }

                if ( exportingCell.IsColorized ) {
                    var style = (XSSFCellStyle)workbook.CreateCellStyle();
                    style.SetFillForegroundColor( exportingCell.Color );
                    style.FillPattern = FillPattern.SolidForeground;
                    cell.CellStyle = style;
                }

                if ( exportingCell.CellType != CellType.Unknown ) {
                    cell.SetCellType( exportingCell.CellType );
                }

                switch ( exportingCell.CellType ) {

                    case CellType.String:
                        cell.SetCellValue( exportingCell.GetStringValue() );
                        break;

                    case CellType.Numeric:
                        cell.SetCellValue( exportingCell.GetDoubleValue() );
                        break;

                    case CellType.Boolean:
                        cell.SetCellValue( exportingCell.GetBooleanValue() );
                        break;

                    case CellType.Unknown:
                        cell.SetCellValue( exportingCell.GetDateTimeValue() );
                        break;
                }
            }

            workbook.Write( stream, stayOpen );

            if ( stayOpen ) {
                stream.Position = 0;
            }
        }
    }
}
