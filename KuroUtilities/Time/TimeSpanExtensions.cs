using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroUtilities.Time
{
    /// <summary>
    /// TimeSpanの拡張メソッドを提供するクラスです
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// このインスタンスの値に指定された日数を加算した新しいTimeSpanを返します。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="value">整数部と小数部からなる時間数</param>
        /// <returns></returns>
        public static TimeSpan AddDays(this TimeSpan timeSpan, double value) { return timeSpan.Add(TimeSpan.FromDays(value)); }
        /// <summary>
        /// このインスタンスの値に指定された時間数を加算した新しいTimeSpanを返します。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="value">整数部と小数部からなる時間数</param>
        /// <returns></returns>
        public static TimeSpan AddHours(this TimeSpan timeSpan,double value) { return timeSpan.Add(TimeSpan.FromHours(value)); }
        /// <summary>
        /// このインスタンスの値に指定された分数を加算した新しいTimeSpanを返します。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="value">整数部と小数部からなる分数</param>
        /// <returns></returns>
        public static TimeSpan AddMinutes(this TimeSpan timeSpan, double value) { return timeSpan.Add(TimeSpan.FromMinutes(value)); }
        /// <summary>
        /// このインスタンスの値に指定された秒数を加算した新しいTimeSpanを返します。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="value">整数部と小数部からなる分数</param>
        /// <returns></returns>
        public static TimeSpan AddSeconds(this TimeSpan timeSpan, double value) { return timeSpan.Add(TimeSpan.FromSeconds(value)); }
        /// <summary>
        /// このインスタンスの値に指定されたミリ秒数を加算した新しいTimeSpanを返します。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="value">整数部と小数部からなる時間数</param>
        /// <returns></returns>
        public static TimeSpan AddMilliseconds(this TimeSpan timeSpan, double value) { return timeSpan.Add(TimeSpan.FromMinutes(value)); }
    }
}
