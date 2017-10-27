using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KuroUtilities.Icon
{
    /// <summary>
    /// Iconの拡張メソッドを提供するクラスです。
    /// </summary>
    public static class IconExtensions
    {
        /// <summary>
        /// System.Drawing.IconをBitmapSourceに変換します。
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// リソースを開放する
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        /// <summary>
        /// System.Drawing.BitmapをBitmapSourceに変換します。
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(this Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
    }
}
