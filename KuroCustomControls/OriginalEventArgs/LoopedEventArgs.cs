using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroCustomControls
{
    public delegate void LoopedEventHandler(object sender, LoopedEventArgs e);
    /// <summary>
    /// ループした時に、最大値を超えてループしたかどうかのイベントデータを提供します。
    /// </summary>
    public class LoopedEventArgs : EventArgs
    {
        private readonly bool isMaxOver;
        /// <summary>
        /// 最大値を超えてループしたか(falseなら最小値を下回ってのループ)
        /// </summary>
        public bool IsMaxOver
        {
            get { return isMaxOver; }
        }
        public LoopedEventArgs(bool isMaxOver)
        {
            this.isMaxOver = isMaxOver;
        }
    }
}
