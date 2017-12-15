using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroUtilities.FileSystemInfo
{
    /// <summary>
    /// DirectoryInfoの拡張メソッドを提供するクラスです。
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// DirectoryInfoが存在する場合はtrueを、存在しないかDirectoryInfoがnullの場合はfalseを返します。
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool IsNotNullOrExists(this DirectoryInfo directory)
        {
            if (directory != null) return directory.Exists;
            return false;
        }
    }
}
