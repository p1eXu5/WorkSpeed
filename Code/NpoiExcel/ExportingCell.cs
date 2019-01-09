using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NpoiExcel
{
    public class ExportingCell : IExportingCell
    {
        private readonly string _stringValue;
        private readonly double _doubleValue;
        private readonly bool _boolValue;
        private readonly DateTime _dateTimeValue;

        #region Ctor

        private ExportingCell ( Color color )
        {
            Color = color.Equals( default( Color ) ) ? new XSSFColor( new HSSFColor.Automatic().RGB ) : new XSSFColor( color );
        }

        private ExportingCell ( byte[] color )
        {
            if ( color != null && color.Length != 3 ) throw new ArgumentException( @"Dimension of color not equals three.", nameof ( color ) );

            Color = new XSSFColor( color ?? new HSSFColor.Automatic().RGB );
        }

        private ExportingCell ( int row, int column, Color color )
            : this ( color )
        {
            if ( row < 0 ) throw new ArgumentException( @"Row cannot be less than zero." );
            if ( column < 0 && column > short.MaxValue ) throw new ArgumentException( $"Column cannot be less than zero or greater than short.MaxValue." );

            Row = row;
            Column = (short)column;
        } 

        private ExportingCell ( int row, int column, byte[] color )
            : this ( color )
        {
            if ( row < 0 ) throw new ArgumentException( @"Row cannot be less than zero." );
            if ( column < 0 && column > short.MaxValue ) throw new ArgumentException( $"Column cannot be less than zero or greater than short.MaxValue." );

            Row = row;
            Column = (short)column;
        } 

        public ExportingCell ( string value, int row, int column, Color color = default( Color ) )
            : this ( row, column, color )
        {
            _stringValue = value;
            CellType = CellType.String;
        }

        public ExportingCell ( string value, int row, int column, byte[] color = null )
            : this ( row, column, color )
        {
            _stringValue = value;
            CellType = CellType.String;
        }

        public ExportingCell ( double value, int row, int column, Color color = default( Color ) )
            : this ( row, column, color )
        {
            _doubleValue = value;
            _stringValue = value.ToString( CultureInfo.CurrentCulture );
            CellType = CellType.Numeric;
        }

        public ExportingCell ( double value, int row, int column, byte[] color = null )
            : this ( row, column, color )
        {
            _doubleValue = value;
            _stringValue = value.ToString( CultureInfo.CurrentCulture );
            CellType = CellType.Numeric;
        }

        public ExportingCell ( bool value, int row, int column, Color color = default( Color ) )
            : this ( row, column, color )
        {
            _boolValue = value;
            _stringValue = value ? "Да" : "Нет";
            CellType = CellType.Boolean;
        }

        public ExportingCell ( bool value, int row, int column, byte[] color = null )
            : this ( row, column, color )
        {
            _boolValue = value;
            _stringValue = value ? "Да" : "Нет";
            CellType = CellType.Boolean;
        }

        public ExportingCell ( DateTime value, int row, int column, Color color = default( Color ) )
            : this ( row, column, color )
        {
            _dateTimeValue = value;
            _stringValue = value.ToString( CultureInfo.CurrentCulture );
            CellType = CellType.Unknown;
        }

        public ExportingCell ( DateTime value, int row, int column, byte[] color = null )
            : this ( row, column, color )
        {
            _dateTimeValue = value;
            _stringValue = value.ToString( CultureInfo.CurrentCulture );
            CellType = CellType.Unknown;
        }

        #endregion

        public int Row { get; }
        public int Column { get; }
        public XSSFColor Color { get; }
        public CellType CellType { get; }

        public bool IsColorized => Color != null;

        public double GetDoubleValue ()
        {
            if ( CellType == CellType.Numeric ) {
                return _doubleValue;
            }

            return default( double );
        }

        public string GetStringValue ()
        {
            return _stringValue;
        }

        public bool GetBooleanValue ()
        {
            if ( CellType == CellType.Boolean ) {
                return _boolValue;
            }

            return default( bool );
        }

        public DateTime GetDateTimeValue ()
        {
            if ( CellType == CellType.Unknown ) {

                return _dateTimeValue;
            }

            return new DateTime( 1900, 1, 1 );
        }

        public object GetValue ()
        {
            switch ( CellType ) {
                case CellType.String: return _stringValue;
                case CellType.Numeric: return _doubleValue;
                case CellType.Boolean: return _boolValue;
                case CellType.Unknown: return _dateTimeValue;
                default:
                    throw new InvalidOperationException( "Inner CellType is different." );
            }
        }
    }
}
