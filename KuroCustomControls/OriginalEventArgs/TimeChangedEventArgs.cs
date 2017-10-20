using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroCustomControls
{
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs e);
    /// <summary>
    /// Time変更時のイベントデータを提供します。
    /// </summary>
    public class TimeChangedEventArgs : EventArgs
    {
        private readonly TimeSpan newTime;
        /// <summary>
        /// 変更後のTimeSpan
        /// </summary>
        public TimeSpan NewTime
        {
            get { return newTime; }
        }
        private readonly TimeSpan oldTime;
        /// <summary>
        /// 変更前のTimeSpan
        /// </summary>
        public TimeSpan OldTime
        {
            get { return oldTime; }
        }
        /// <summary>
        /// 変更後と変更前のTimeSpanの差(NewTim - OldTime)
        /// </summary>
        public TimeSpan SubtractedTime
        {
            get { return NewTime - OldTime; }
        }
        public TimeChangedEventArgs(TimeSpan newTime,TimeSpan oldTime)
        {
            this.newTime = newTime;
            this.oldTime = oldTime;
        }
    }
}
