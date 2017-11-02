using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KuroUtilities.Win32
{
    /// <summary>
    /// SHFILEINFO構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct SHFILEINFO
    {
        /// <summary>
        /// ファイルを表すアイコンのハンドル。不要になったときにDestroyIconでこのハンドルを破棄する必要がある。
        /// </summary>
        public IntPtr hIcon;
        /// <summary>
        /// システムイメージリスト内のアイコンイメージのインデックス。
        /// </summary>
        public IntPtr iIcon;
        /// <summary>
        /// ファイルオブジェクトの属性を示す値の配列。
        /// </summary>
        public uint dwAttributes;
        /// <summary>
        /// Windowsシェルに表示されるファイルの名前、またはファイルを表すアイコンを含むファイルのパスとファイル名を含む文字列。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        /// <summary>
        /// ファイルの種類を表す文字列。
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    /// <summary>
    /// 
    /// </summary>
    public static class SHGetFileInfoEx
    {
        const uint SHGFI_LARGEICON = 0x00000000;//大きなアイコンの取得
        const uint SHGFI_SMALLICON = 0x00000001;//小さなアイコンの取得
        const uint SHGFI_USEFILEATTRIBUTES = 0x00000010;//アクセス禁止
        const uint SHGFI_ICON = 0x00000100;
        /// <summary>
        /// SHGetFileInfoを呼び出す
        /// </summary>
        /// <param name="pszPath">アプリケーション・アイコンを含むファイルへのパス</param>
        /// <param name="dwFileAttributes">uFlagsにSHGFI_USEFILEATTRIBUTESを指定した場合に使用する</param>
        /// <param name="psfi">SHFILEINFO構造体のデータ。ここからアイコン・リソース情報が得られる</param>
        /// <param name="cbFileInfo">SHFILEINFO構造体のサイズ(バイト数)</param>
        /// <param name="uFlags">取得するファイル情報の内容を細かく指定するためのフラグ</param>
        /// <returns></returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyIcon(IntPtr hIcon);


        public static Bitmap GetAssociatedImage(string path,bool isLarge)
        {
            SHFILEINFO fileInfo = new SHFILEINFO();
            uint flags = SHGFI_ICON;
            if (!isLarge) flags |= SHGFI_SMALLICON;
            if (!File.Exists(path) && !Directory.Exists(path)) flags |= SHGFI_USEFILEATTRIBUTES;
            try
            {
                SHGetFileInfo(path, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flags);
                if (fileInfo.hIcon == IntPtr.Zero)
                    return null;
                else
                    return System.Drawing.Icon.FromHandle(fileInfo.hIcon).ToBitmap(); ;
            }
            finally
            {
                if (fileInfo.hIcon != IntPtr.Zero)
                    DestroyIcon(fileInfo.hIcon);
            }
        }

        /// <summary>
        /// 指定したファイルパスに関連付けされたアイコンイメージを取得する。
        /// </summary>
        /// <remarks>
        /// このメソッドは、ファイルの存在チェックを行ない、指定されなかった第３パラメータの
        /// 値を決定する。
        /// </remarks>
        /// <param name="path">アイコンイメージ取得対象のファイルのパス</param>
        /// <param name="isLarge">大きいアイコンを取得するとき true、小さいアイコンを取得するとき false</param>
        /// <returns>取得されたアイコンのビットマップイメージを返す。</returns>
        public static Image FileAssociatedImage(string path, bool isLarge)
        {
            return FileAssociatedImage(path, isLarge, File.Exists(path));
        }

        /// <summary>
        /// 指定したファイルパスに関連付けされたアイコンイメージを取得する。
        /// </summary>
        /// <param name="path">アイコンイメージ取得対象のファイルのパス</param>
        /// <param name="isLarge">
        /// 大きいアイコンを取得するとき true、小さいアイコンを取得するとき false
        /// </param>
        /// <param name="isExist">
        /// ファイルが実在するときだけ動作させるとき true、実在しなくて動作させるとき false
        /// </param>
        /// <returns>取得されたアイコンのビットマップイメージを返す。</returns>
        public static Image FileAssociatedImage(string path, bool isLarge, bool isExist)
        {
            SHFILEINFO fileInfo = new SHFILEINFO();
            uint flags = SHGFI_ICON;
            if (!isLarge) flags |= SHGFI_SMALLICON;
            if (!isExist) flags |= SHGFI_USEFILEATTRIBUTES;
            try
            {
                SHGetFileInfo(path, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flags);
                if (fileInfo.hIcon == IntPtr.Zero)
                    return null;
                else
                    return System.Drawing.Icon.FromHandle(fileInfo.hIcon).ToBitmap();
            }
            finally
            {
                if (fileInfo.hIcon != IntPtr.Zero)
                    DestroyIcon(fileInfo.hIcon);
            }
        }
    }
}