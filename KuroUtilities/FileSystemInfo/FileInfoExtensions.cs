using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroUtilities.FileSystemInfo
{
    /// <summary>
    /// FileInfoの拡張メソッドを提供するクラスです。
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// FileInfoが存在する場合はtrueを、存在しないかFileInfoがnullの場合はfalseを返します。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsNotNullOrExists(this FileInfo file)
        {
            if(file != null) return file.Exists;
            return false;
        }
    }
}
