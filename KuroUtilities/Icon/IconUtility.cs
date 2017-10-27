using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace KuroUtilities.Icon
{
    /// <summary>
    /// Icon系のUtilityメソッドを提供するクラスです。
    /// </summary>
    public static class IconUtility
    {
        /// <summary>
        /// 指定したパスのファイルに関連付けられたアイコンをIcon型で取得します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static System.Drawing.Icon GetFileAssociatedIcon(string filePath)
        {
            return System.Drawing.Icon.ExtractAssociatedIcon(filePath);
        }
        /// <summary>
        ///指定したファイルに関連付けられたアイコンをIcon型で取得します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static System.Drawing.Icon GetFileAssociatedIcon(FileInfo file)
        {
            return GetFileAssociatedIcon(file.FullName);
        }
        /// <summary>
        /// 指定したパスのファイルに関連付けられたアイコンをBitmapSource型で取得します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static BitmapSource GetFileAssociatedBitmap(string filePath)
        {
            return GetFileAssociatedIcon(filePath).ToBitmapSource();
        }
        /// <summary>
        ///指定したファイルに関連付けられたアイコンをBitmapSource型で取得します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static BitmapSource GetFileAssociatedBitmap(FileInfo file)
        {
            return GetFileAssociatedIcon(file.FullName).ToBitmapSource();
        }
    }
}
