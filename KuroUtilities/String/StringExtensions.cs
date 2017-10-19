using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KuroUtilities.String
{
    /// <summary>
    /// stringの拡張メソッドを提供するクラスです。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列がnullまたはstring.Empty文字列であるかどうかを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str) { return string.IsNullOrEmpty(str); }
        /// <summary>
        /// 文字列がnullまたは空であるか、空白文字だけで構成されているかどうかを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str) { return string.IsNullOrWhiteSpace(str); }
    }
}
