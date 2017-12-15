using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        [Description("ノードのヘッダーに表示するアイコンを指定します。これはHeaderImageのSourceプロパティにバインドされます。"), Category("共通")]
        public ImageSource HeaderIcon
        {
            get { return (ImageSource)GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconProperty =
            DependencyProperty.Register("HeaderIcon", typeof(ImageSource), typeof(BaseNode), new PropertyMetadata(null));
        #endregion
        #region HeaderIconWidthプロパティ
        [Description("ヘッダーアイコンの幅を指定します。これはHeaderImageのWidthプロパティにバインドされます。"), Category("レイアウト")]
        public double HeaderIconWidth
        {
            get { return (double)GetValue(HeaderIconWidthProperty); }
            set { SetValue(HeaderIconWidthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIconWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconWidthProperty =
            DependencyProperty.Register("HeaderIconWidth", typeof(double), typeof(BaseNode), new FrameworkPropertyMetadata(18D, FrameworkPropertyMetadataOptions.Inherits));
        #endregion
        #region HeaderIconHeightプロパティ
        [Description("ヘッダーアイコンの高さを指定します。これはHeaderImageのHeightプロパティにバインドされます。"), Category("レイアウト")]
        public double HeaderIconHeight
        {
            get { return (double)GetValue(HeaderIconHeightProperty); }
            set { SetValue(HeaderIconHeightProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderIconHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderIconHeightProperty =
            DependencyProperty.Register("HeaderIconHeight", typeof(double), typeof(BaseNode), new FrameworkPropertyMetadata(15D, FrameworkPropertyMetadataOptions.Inherits));
        #endregion
        #region HeaderTextプロパティ
        [Description("ノードのヘッダーに表示するテキストを指定します。これはHeaderTextBlockのTextプロパティにバインドされます。"), Category("共通")]
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(BaseNode), new PropertyMetadata(""));
        #endregion
        [Description("ヘッダーに表示するパネルで、HeaderImageとHeaderTextBlockのコンテナです。(読取り専用)"),Category("共通")]
        public StackPanel HeaderPanel { get; } = new StackPanel() { Orientation = Orientation.Horizontal };
        [Description("ヘッダーに表示するImageです。(読取り専用)"), Category("共通")]
        public Image HeaderImage { get; } = new Image() { Width = 15, Height = 18 };
        [Description("ヘッダーに表示するTextBlockです。(読取り専用)"), Category("共通")]
        public TextBlock HeaderTextBlock { get; } = new TextBlock() { VerticalAlignment = VerticalAlignment.Center };
        #endregion
        #region メソッド
        public BaseNode()
        {
            this.HeaderPanel.Children.Add(this.HeaderImage);
            this.HeaderPanel.Children.Add(this.HeaderTextBlock);
            this.SetBinding(HeaderProperty, new Binding(nameof(HeaderPanel)) { Source = this });
            HeaderImage.SetBinding(Image.SourceProperty, new Binding(nameof(HeaderIcon)) { Source =this });
            HeaderImage.SetBinding(Image.WidthProperty, new Binding(nameof(HeaderIconWidth)) { Source = this });
            HeaderImage.SetBinding(Image.HeightProperty, new Binding(nameof(HeaderIconHeight)) { Source = this });
            HeaderTextBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(HeaderText)) { Source = this });
        }
        #endregion
    }
}
