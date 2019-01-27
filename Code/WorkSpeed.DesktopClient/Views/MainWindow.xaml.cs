using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkSpeed.DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr hWnd;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded ( object sender, RoutedEventArgs e )
        {
            hWnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hWnd)?.AddHook(WindowHook);
        }

        private IntPtr WindowHook ( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lPatam, ref bool handled )
        {
            if (msg == ApiCodes.WM_SYSCOMMAND)
            {
                //if ( wParam.ToInt32() == ApiCodes.SC_MAXIMIZE ) {

                //    WindowStyle = WindowStyle.SingleBorderWindow;
                //    WindowState = WindowState.Maximized;
                //    handled = true;
                //}

                //if ( wParam.ToInt32() == ApiCodes.SC_MINIMIZE ) {

                //    WindowStyle = WindowStyle.SingleBorderWindow;
                //    WindowState = WindowState.Minimized;
                //    handled = true;

                //}
                if (wParam.ToInt32() == ApiCodes.SC_RESTORE) {

                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.None;

                    handled = true;
                }
            }
            return IntPtr.Zero;
        }

        public void Minimize ()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Minimized;
        }

        public void MaximizeTrigger ()
        {
            switch ( WindowState ) {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
            }
        }

        [ DllImport( "User32.dll" ) ]
        static extern IntPtr SendMessage ( IntPtr hwnd, uint msg, uint wParam, IntPtr lParam );

        private static class ApiCodes
        {
            public const uint WM_SYSCOMMAND = 0x0112;
            public const uint SC_CLOSE = 0xF060;
            public const uint SC_MAXIMIZE = 0xF030;
            public const uint SC_MINIMIZE = 0xF020;
            public const uint SC_RESTORE = 0xF120;
        }
    }
}
