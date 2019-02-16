using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.Converters
{
    public class ImageConverter : IValueConverter
    {
        private static readonly PixelFormat _pixelFormat;

        static ImageConverter ()
        {
            _pixelFormat = new PixelFormat();
            _pixelFormat = PixelFormats.Bgr24;
        }

        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( !( value is Avatar avatar )) return DependencyProperty.UnsetValue;

            var bitmapSource = BitmapSource.Create( avatar.Width, avatar.Height, 96, 96, _pixelFormat, null, avatar.Picture, avatar.Stride );
            return (ImageSource)bitmapSource;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
