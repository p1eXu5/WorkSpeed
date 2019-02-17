using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.Converters
{
    public class AbbreviationsConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( !(value is Appointment appointment) || string.IsNullOrWhiteSpace( appointment.Abbreviations ) ) return DependencyProperty.UnsetValue;

            StringBuilder sb = new StringBuilder();

            using ( var enumerator = appointment.Abbreviations.GetEnumerator() ) {

                while ( enumerator.MoveNext() ) {
                    if ( enumerator.Current == ';' ) { break; }
                    sb.Append( enumerator.Current );
                }
            }

            return sb.ToString();
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
