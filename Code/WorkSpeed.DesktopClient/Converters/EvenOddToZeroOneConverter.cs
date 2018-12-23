using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WorkSpeed.DesktopClient.Converters
{
    public class EvenOddToZeroOneConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            int intValue = 0;

            try {
                intValue = System.Convert.ToInt32( value );
            }
            catch { }

            if ( intValue % 2 == 0 ) return 0;

            return 1;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
