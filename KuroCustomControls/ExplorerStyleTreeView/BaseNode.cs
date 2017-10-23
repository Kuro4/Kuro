using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KuroCustomControls
{
    /// <summary>
    /// TreeViewItemを継承し、Nodeのベースとなるクラス
    /// </summary>
    public class BaseNode : TreeViewItem
    {
        /// <summary>
        /// 自身が持つFileSystemInfo(DirectoryInfoかFileInfo)
        /// </summary>
        public FileSystemInfo _Info { get; set; }
        /// <summary>
        /// Headerに表示するアイコン
        /// </summary>
        public Image _HeaderImage { get; private set; } = new Image();
        /// <summary>
        /// Headerに表示するテキスト
        /// </summary>
        public TextBlock _HeaderText { get; private set; } = new TextBlock();
        /// <summary>
        /// Headerに表示するアイコンとテキストを持つパネル
        /// </summary>
        public StackPanel _HeaderPanel { get; private set; } = new StackPanel() { Orientation = Orientation.Horizontal };
        public BaseNode()
        {
            _HeaderPanel.Children.Add(_HeaderImage);
            _HeaderPanel.Children.Add(_HeaderText);
            this.Header = _HeaderPanel;

            //これを設定しとかないとバインドエラーが出る
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
        }
    }
}
