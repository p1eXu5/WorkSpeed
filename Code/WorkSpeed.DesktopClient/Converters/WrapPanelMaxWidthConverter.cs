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
    public class WrapPanelMaxWidthConverter : IMultiValueConverter
    {
        private const double _default = 577.0;

        /// <summary>
        ///     Gets: [0] - items count;
        ///           [1] - employee card width
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert ( object[] values, Type targetType, object parameter, CultureInfo culture )
        {
            if ( !(values[ 0 ] is double)
                && !(values[ 1 ] is double) ) {
                return _default;
            }

            var count = System.Convert.ToDouble( values[ 0 ] );
            var width = System.Convert.ToDouble( values[ 1 ] );
            var horElemCount = Math.Round( 0.38 * Math.Sqrt( count ) * 2 );

            return horElemCount * width;
        }

        public object[] ConvertBack ( object value, Type[] targetTypes, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
