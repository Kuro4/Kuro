using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroUtilities.LINQ
{
    /// <summary>
    /// IEnumerableの拡張メソッドを提供するクラスです。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// IEnumerableに要素が含まれているならtrueを、含まれていないかIEnumerableがnullの場合はfalseを返します。
        /// </summary>
        /// <param name="enumrable"></param>
        /// <returns></returns>
        public static bool IsNotNullOrAny(this IEnumerable enumrable)
        {
            if (enumrable != null) return enumrable.Cast<object>().Any();
            return false;
        }
        /// <summary>
        /// IEnumerableから指定した型のリストを作成します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumrable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable enumrable)
        {
            return  enumrable.Cast<T>().ToList();
        }
    }
}
