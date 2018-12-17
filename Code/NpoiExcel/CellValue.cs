using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NpoiExcel
{
    public struct CellValue
    {
        private readonly string _stringValue;
        private readonly double _doubleValue;
        private readonly bool _boolValue;
        private readonly DateTime _dateTimeValue;

        public CellValue (ICell cell)
        {
            if (null == cell) {
                
                _stringValue = default (string);
                _doubleValue = default (double);
                _boolValue = default (bool);
                _dateTimeValue = default (DateTime);
            }
            else {
                int days;
                switch (cell.CellType) {
                    case CellType.Blank:
                        _stringValue = default (string);
                        _doubleValue = default (double);
                        _boolValue = default (bool);
                        _dateTimeValue = default (DateTime);
                        break;

                    case CellType.Boolean:
                        _boolValue = cell.BooleanCellValue;
                        _stringValue = _boolValue ? "Да" : "Нет";
                        _doubleValue = _boolValue ? 1.0 : 0.0;
                        _dateTimeValue = default (DateTime);
                        break;

                    case CellType.Numeric:
                        _doubleValue = cell.NumericCellValue;
                        _boolValue = !_doubleValue.Equals (0.0);
                        _stringValue = _doubleValue.ToString(CultureInfo.CurrentCulture);

                        try {
                            days = Convert.ToInt32 (_doubleValue);
                            _dateTimeValue = days != 0 ? new DateTime (1900, 1, 1).AddDays (days - 2) : default(DateTime);
                        }
                        catch (OverflowException) {
                            _dateTimeValue = default(DateTime);
                        }
                        catch (ArgumentOutOfRangeException) {
                            _dateTimeValue = default(DateTime);
                        }
                        break;

                    case CellType.Error:
                        _doubleValue = cell.ErrorCellValue;
                        _boolValue = !_doubleValue.Equals (0.0);
                        _stringValue = _doubleValue.ToString(CultureInfo.CurrentCulture);

                        try {
                            days = Convert.ToInt32 (_doubleValue);
                            _dateTimeValue = days != 0 ? new DateTime (1900, 1, 1).AddDays (days - 2) : default(DateTime);
                        }
                        catch (OverflowException) {
                            _dateTimeValue = default(DateTime);
                        }
                        catch (ArgumentOutOfRangeException) {
                            _dateTimeValue = default(DateTime);
                        }
                        break;

                    case CellType.Formula:
                        _stringValue = cell.CellFormula;
                        _doubleValue = default (double);
                        _boolValue = default (bool);
                        _dateTimeValue = default (DateTime);
                        break;

                    default:
                        _stringValue = cell.StringCellValue;
                        var trimmedString = _stringValue.Trim();

                        try {
                            _boolValue = bool.Parse (_stringValue.Trim());

                            if (_boolValue) {
                                _doubleValue = 1.0;
                            }

                            _doubleValue = 0.0;
                        }
                        catch (Exception) {

                            if (trimmedString.ToUpperInvariant().Equals ("ДА")
                                || trimmedString.ToUpperInvariant().Equals ("YES")) {
                                _boolValue = true;
                                _doubleValue = 1.0;
                            }
                            else {
                                _boolValue = default(bool);

                                try {
                                    _doubleValue = double.Parse (trimmedString);

                                    if (!_doubleValue.Equals (0.0) && !_boolValue) {
                                        _boolValue = true;
                                    }
                                }
                                catch (Exception) {
                                    _doubleValue = default(double);
                                }
                            }
                        }

                        try {
                            _dateTimeValue = DateTime.Parse (_stringValue);
                        }
                        catch (FormatException) {
                            try {
                                _dateTimeValue = cell.DateCellValue;
                                _stringValue = _dateTimeValue.ToString (CultureInfo.CurrentCulture);
                            }
                            catch {
                                try {
                                    days = Convert.ToInt32 (_doubleValue);
                                    if (days != 0) {
                                        _dateTimeValue = new DateTime (1900, 1, 1).AddDays (days - 2);
                                        _stringValue = _dateTimeValue.ToString (CultureInfo.CurrentCulture);
                                    }
                                    else {
                                        _dateTimeValue = default(DateTime);
                                    }
                                }
                                catch(OverflowException) {
                                    _dateTimeValue = default(DateTime);
                                }
                                catch (ArgumentOutOfRangeException) {
                                    _dateTimeValue = default(DateTime);
                                }
                            }
                        }

                        break;
                }
            }
        }

        public static implicit operator int (CellValue value)
        {
            if ( value._doubleValue > int.MaxValue ) return int.MaxValue;
            if ( value._doubleValue < int.MinValue ) return int.MinValue;
            return Convert.ToInt32( value._doubleValue );
        }

        public static implicit operator double (CellValue value)
        {
            return value._doubleValue;
        }

        public static implicit operator string (CellValue value)
        {
            return value._stringValue;
        }

        public static implicit operator bool (CellValue value)
        {
            return value._boolValue;
        }

        public static implicit operator DateTime (CellValue value)
        {
            return value._dateTimeValue;
        }

        public override string ToString()
        {
            return _stringValue;
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals (null, obj)) return false;
            return obj is CellValue other && Equals (other);
        }

        public bool Equals (CellValue other)
        {
            return _stringValue.Equals (other._stringValue);
        }

        public override int GetHashCode()
        {
            return _stringValue.GetHashCode();
        }
    }

}
