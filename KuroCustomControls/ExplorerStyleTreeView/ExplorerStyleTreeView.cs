using KuroUtilities.Icon;
using KuroUtilities.LINQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KuroCustomControls
{
    public class ExplorerStyleTreeView : Control
    {
        static ExplorerStyleTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerStyleTreeView),
                new FrameworkPropertyMetadata(typeof(ExplorerStyleTreeView)));
        }
        #region プロパティ
        #region RootDirectoriesプロパティ
        [Description("ルートディレクトリをIEnamrableで指定します。指定しなければ準備ができているドライブをルートにします。"),Category("共通")]
        public IEnumerable<DirectoryNode> RootDirectories
        {
            get { return (IEnumerable<DirectoryNode>)GetValue(RootDirectoriesProperty); }
            set { SetValue(RootDirectoriesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for RootDirectories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RootDirectoriesProperty =
            DependencyProperty.Register("RootDirectories", typeof(IEnumerable<DirectoryNode>), typeof(ExplorerStyleTreeView), new PropertyMetadata(null, OnRootDirectoriesChanged));
        #endregion
        #region OpenFolderIconプロパティ
        [Description("ノード展開時に表示するフォルダのアイコンです。設定しなければ関連付けられたアイコンを設定します。"), Category("共通")]
        public ImageSource OpenFolderIcon
        {
            get { return (ImageSource)GetValue(OpenFolderIconProperty); }
            set { SetValue(OpenFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OpenFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenFolderIconProperty =
            DependencyProperty.Register("OpenFolderIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new PropertyMetadata(null, OnChildrenRelatePropertyChanged));
        #endregion
        #region CloseFolderIcon
        [Description("ノード格納時に表示するフォルダのアイコンです。設定しなければ関連付けられたアイコンを設定します。"), Category("共通")]
        public ImageSource CloseFolderIcon
        {
            get { return (ImageSource)GetValue(CloseFolderIconProperty); }
            set { SetValue(CloseFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CloseFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseFolderIconProperty =
            DependencyProperty.Register("CloseFolderIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new PropertyMetadata(null, OnChildrenRelatePropertyChanged));
        #endregion
        #region FolderErrorIconプロパティ
        [Description("ディレクトリが見つからなかった時のエラーアイコンです。"), Category("共通")]
        public ImageSource FolderErrorIcon
        {
            get { return (ImageSource)GetValue(FolderErrorIconProperty); }
            set { SetValue(FolderErrorIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ErrorIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderErrorIconProperty =
            DependencyProperty.Register("FolderErrorIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new FrameworkPropertyMetadata(Properties.Resources.FolderError_16x.ToImageSource(), FrameworkPropertyMetadataOptions.Inherits, OnChildrenRelatePropertyChanged));
        #endregion
        #region FileIconプロパティ
        [Description("ファイルのアイコンです。設定しなければ標準のアイコンになります。"), Category("共通")]
        public ImageSource FileIcon
        {
            get { return (ImageSource)GetValue(FileIconProperty); }
            set { SetValue(FileIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIconProperty =
            DependencyProperty.Register("FileIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new PropertyMetadata(null, OnChildrenRelatePropertyChanged));
        #endregion
        #region FileErrorIconプロパティ
        [Description("ファイルが見つからなかった時のエラーアイコンです。"), Category("共通")]
        public ImageSource FileErrorIcon
        {
            get { return (ImageSource)GetValue(FileErrorIconProperty); }
            set { SetValue(FileErrorIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FileErrorIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileErrorIconProperty =
            DependencyProperty.Register("FileErrorIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new FrameworkPropertyMetadata(Properties.Resources.FileError_16x.ToImageSource(), FrameworkPropertyMetadataOptions.Inherits, OnChildrenRelatePropertyChanged));
        #endregion
        #region SearchPatternプロパティ
        [Description("ファイルの検索パターンを正規表現で指定します。既定値は「..*」(全検索)。\r\n「\\.csv$」のようにするとcsv拡張子ファイルのみ検索します。\r\n複数指定の場合は「\\.csv$|\\.txt$」のように記述します。"), Category("共通")]
        public string SearchPattern
        {
            get { return (string)GetValue(SearchPatternProperty); }
            set { SetValue(SearchPatternProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SearchPattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchPatternProperty =
            DependencyProperty.Register("SearchPattern", typeof(string), typeof(ExplorerStyleTreeView), new PropertyMetadata(@"..*", OnChildrenRelatePropertyChanged));
        #endregion
        #region IsAlwaysUpdateNodeプロパティ
        [Description("ノードを展開する毎に子ノードを更新するかどうかを指定します。既定値はfalseです。"), Category("共通")]
        public bool IsAlwaysUpdateNode
        {
            get { return (bool)GetValue(IsAlwaysUpdateNodeProperty); }
            set { SetValue(IsAlwaysUpdateNodeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsAlwaysUpdateNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAlwaysUpdateNodeProperty =
            DependencyProperty.Register("IsAlwaysUpdateNode", typeof(bool), typeof(ExplorerStyleTreeView), new PropertyMetadata(false, OnChildrenRelatePropertyChanged));
        #endregion
        #region SelectedNodeプロパティ
        private static readonly DependencyPropertyKey SelectedNodePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "SelectedNode",
                typeof(BaseNode),
                typeof(ExplorerStyleTreeView),
                new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedNodeProperty = SelectedNodePropertyKey.DependencyProperty;
        [Description("現在選択中のNode(DirectoryNodeかFileNode)です。使用する際は型判定して下さい。(読取専用)"), Category("共通")]
        public BaseNode SelectedNode
        {
            get { return (BaseNode)GetValue(SelectedNodeProperty); }
            private set { this.SetValue(SelectedNodePropertyKey, value); }
        }
        #endregion
        #region SelectedNodeValueプロパティ
        private static readonly DependencyPropertyKey SelectedNodeValuePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "SelectedNodeValue",
                typeof(FileSystemInfo),
                typeof(ExplorerStyleTreeView),
                new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedNodeValueProperty = SelectedNodeValuePropertyKey.DependencyProperty;
        [Description("現在選択中のNodeのディレクトリかファイルです。使用する際は型判定して下さい。(読取専用)"), Category("共通")]
        public FileSystemInfo SelectedNodeValue
        {
            get { return (FileSystemInfo)GetValue(SelectedNodeValueProperty); }
            private set { this.SetValue(SelectedNodeValuePropertyKey, value); }
        }
        #endregion
        #endregion
        #region メソッド
        /// <summary>
        /// イベントの登録・解除
        /// </summary>
        public ExplorerStyleTreeView()
        {
            this.RootDirectoriesUpdate();
        }
        /// <summary>
        /// ルートディレクトリを更新する。
        /// ルートディレクトリが設定されていなければ準備ができているドライブをルートにする。
        /// </summary>
        public void RootDirectoriesUpdate()
        {
            if (!this.RootDirectories.IsNotNullOrAny())
            {
                this.RootDirectories = DriveInfo.GetDrives().Where(x => x.IsReady).Select(x => new DirectoryNode(x.RootDirectory));
            }
        }
        /// <summary>
        /// 子要素へ関係するプロパティを伝搬させて更新する。
        /// </summary>
        private void ChildrenPropertyUpdate()
        {
            foreach(var root in this.RootDirectories)
            {
                root.OpenFolderIcon = this.OpenFolderIcon;
                root.CloseFolderIcon = this.CloseFolderIcon;
                root.FolderErrorIcon = this.FolderErrorIcon;
                root.FileIcon = this.FileIcon;
                root.FileErrorIcon = this.FileErrorIcon;
                root.SearchPattern = this.SearchPattern;
                root.IsAlwaysUpdateNode = this.IsAlwaysUpdateNode;
            }
        }
        #region コールバック
        /// <summary>
        /// ルートディレクトリ変更時、ルートディレクトリ更新処理を行う。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnRootDirectoriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ExplorerStyleTreeView;
            if (!(e.NewValue is IEnumerable<DirectoryNode>)) throw new ArgumentException("RootDirectoriesにはIEnumerable<DirectoryNode>を指定して下さい");
            self.RootDirectoriesUpdate();
        }
        /// <summary>
        /// 子ノードに関係するプロパティ変更時、ChildrenPropertyUpdateを実行して子ノードへ設定を伝播させる。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnChildrenRelatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ExplorerStyleTreeView;
            if (self == null) return;
            self.ChildrenPropertyUpdate();
        }
        #endregion
        #endregion

    }
}
