using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroCustomControls
{
    public delegate void IntChangedEventHandler(object sender, IntChangedEventArgs e);
    /// <summary>
    /// int型を持つイベントデータを提供します。
    /// </summary>
    public class IntChangedEventArgs : EventArgs
    {
        private readonly int newValue;
        /// <summary>
        /// 変更後のValue
        /// </summary>
        public int NewValue
        {
            get { return newValue; }
        }
        private readonly int oldValue;
        /// <summary>
        /// 変更前のValue
        /// </summary>
        public int OldValue
        {
            get { return oldValue; }
        }
        public IntChangedEventArgs(int newValue, int oldValue)
        {
            this.newValue = newValue;
            this.oldValue = oldValue;
        }
    }
}
