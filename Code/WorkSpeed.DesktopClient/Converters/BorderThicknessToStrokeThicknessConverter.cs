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
    public class BorderThicknessToStrokeThicknessConverter : IValueConverter
    {
        /// <summary>
        /// Converts thickness to double by parameter.
        /// </summary>
        /// <param name="value"><see cref="Thickness"/></param>
        /// <param name="targetType"><see cref="double"/></param>
        /// <param name="parameter">"INNER" or "OUTER"</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( targetType != typeof( double ) ) throw new ArgumentException("Target type must be double.");

            Thickness thickness;

            try {
                thickness = ( Thickness )(value ?? throw new ArgumentNullException());
            }
            catch ( InvalidCastException ) {
                thickness = new Thickness();
            }

            switch ( parameter?.ToString().ToUpperInvariant() ) {

                case "INNER":
                    return thickness.Top;
                case "OUTER":
                default:
                    return thickness.Left;
            }
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
