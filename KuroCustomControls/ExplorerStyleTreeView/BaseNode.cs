using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KuroCustomControls
{
    /// <summary>
    /// TreeViewItemを継承し、Nodeのベースとなるクラス
    /// </summary>
    public class BaseNode : TreeViewItem
    {
        #region プロパティ
        #region HeaderIconプロパティ
        [Description("ノードのヘッダーに表示するアイコンを指定します。"),Category("共通")]
        public ImageSource HeaderIcon
        {
            get { return (ImageSource)GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconProperty =
            DependencyProperty.Register("HeaderIcon", typeof(ImageSource), typeof(BaseNode), new PropertyMetadata(null, OnHeaderIconChanged));
        #endregion
        #region HeaderIconWidthプロパティ
        [Description("ヘッダーアイコンの幅を指定します。"), Category("レイアウト")]
        public double HeaderIconWidth
        {
            get { return (double)GetValue(HeaderIconWidthProperty); }
            set { SetValue(HeaderIconWidthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIconWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconWidthProperty =
            DependencyProperty.Register("HeaderIconWidth", typeof(double), typeof(BaseNode), new FrameworkPropertyMetadata(18D, FrameworkPropertyMetadataOptions.Inherits, OnHeaderIconWidthChanged));
        #endregion
        #region HeaderIconHeightプロパティ
        [Description("ヘッダーアイコンの高さを指定します。"), Category("レイアウト")]
        public double HeaderIconHeight
        {
            get { return (double)GetValue(HeaderIconHeightProperty); }
            set { SetValue(HeaderIconHeightProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIconHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconHeightProperty =
            DependencyProperty.Register("HeaderIconHeight", typeof(double), typeof(BaseNode), new FrameworkPropertyMetadata(15D, FrameworkPropertyMetadataOptions.Inherits, OnHeaderIconHeightChanged));
        #endregion
        #region HeaderTextプロパティ
        [Description("ノードのヘッダーに表示するテキストを指定します。"), Category("共通")]
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(BaseNode), new PropertyMetadata("", OnHeaderTextChanged));
        #endregion
        #endregion
        #region フィールド
        /// <summary>
        /// ヘッダーに表示するImage
        /// </summary>
        private Image headerImage = new Image() { Width = 15, Height = 18 };
        /// <summary>
        /// ヘッダーに表示するTextBlock
        /// </summary>
        private TextBlock headerTextBlock = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
        /// <summary>
        /// ヘッダーに表示するStackPanel。headerTextBlockとheaderImageのコンテナ。
        /// </summary>
        private StackPanel headerPanel = new StackPanel() { Orientation = Orientation.Horizontal };
        #endregion
        #region メソッド
        public BaseNode()
        {
            headerPanel.Children.Add(headerImage);
            headerPanel.Children.Add(headerTextBlock);
            this.Header = headerPanel;
        }
        #region コールバック
        private static void OnHeaderIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (BaseNode)d;
            self.headerImage.Source= (ImageSource)e.NewValue;
        }
        private static void OnHeaderIconWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (BaseNode)d;
            self.headerImage.Width = (double)e.NewValue;
        }
        private static void OnHeaderIconHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (BaseNode)d;
            self.headerImage.Height = (double)e.NewValue;
        }
        private static void OnHeaderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (BaseNode)d;
            self.headerTextBlock.Text = e.NewValue.ToString();
        }
        #endregion
        #endregion
    }
}
