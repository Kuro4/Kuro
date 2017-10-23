using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroCustomControls
{
    public delegate void DoubleChangedEventHandler(object sender, DoubleChangedEventArgs e);
    /// <summary>
    /// double型を持つイベントデータを提供します。
    /// </summary>
    public class DoubleChangedEventArgs : EventArgs
    {
        private readonly double newValue;
        /// <summary>
        /// 変更後のValue
        /// </summary>
        public double NewValue
        {
            get { return newValue; }
        }
        private readonly double oldValue;
        /// <summary>
        /// 変更前のValue
        /// </summary>
        public double OldValue
        {
            get { return oldValue; }
        }
        public DoubleChangedEventArgs(double newValue, double oldValue)
        {
            this.newValue = newValue;
            this.oldValue = oldValue;
        }
    }
}
