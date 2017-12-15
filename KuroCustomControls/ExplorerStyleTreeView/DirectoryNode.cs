using KuroUtilities.FileSystemInfo;
using KuroUtilities.Icon;
using KuroUtilities.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KuroCustomControls
{
    /// <summary>
    /// Directory用のNode
    /// </summary>
    public class DirectoryNode : BaseNode
    {
        #region プロパティ
        #region OpenFolderIconプロパティ
        [Description("ノード展開時に表示するフォルダのアイコンです。設定しなければ関連付けられたアイコンを設定します。"), Category("共通")]
        public ImageSource OpenFolderIcon
        {
            get { return (ImageSource)GetValue(OpenFolderIconProperty); }
            set { SetValue(OpenFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OpenFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenFolderIconProperty =
            DependencyProperty.RegisterAttached("OpenFolderIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnIconChanged));
        #endregion
        #region CloseFolderIconプロパティ
        [Description("ノード格納時に表示するフォルダのアイコンです。設定しなければ関連付けられたアイコンを設定します。"), Category("共通")]
        public ImageSource CloseFolderIcon
        {
            get { return (ImageSource)GetValue(CloseFolderIconProperty); }
            set { SetValue(CloseFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CloseFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseFolderIconProperty =
            DependencyProperty.RegisterAttached("CloseFolderIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnIconChanged));
        #endregion
        #region Directoryプロパティ
        [Description("このノードのディレクトリです。string型のパス、もしくはDirectoryInfo型のインスタンスを指定します。"),Category("共通")]
        public DirectoryInfo Directory
        {
            get { return (DirectoryInfo)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Directory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("Directory", typeof(object), typeof(DirectoryNode), new PropertyMetadata(null, OnDirectoryChanged, OnCoerceDirectory));
        #endregion
        #region SearchPatternプロパティ
        [Description("ファイルの検索パターンを正規表現で指定します。既定値は「..*」(全検索)。\r\n「\\.csv$」のようにするとcsv拡張子ファイルのみ検索します。\r\n複数指定の場合は「\\.csv$|\\.txt$」のように記述します。"),Category("共通")]
        public string SearchPattern
        {
            get { return (string)GetValue(SearchPatternProperty); }
            set { SetValue(SearchPatternProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SearchPattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchPatternProperty =
            DependencyProperty.Register("SearchPattern", typeof(string), typeof(DirectoryNode), new FrameworkPropertyMetadata(@"..*",FrameworkPropertyMetadataOptions.Inherits));
        #endregion
        #region IsAlwaysUpdateNodeプロパティ
        [Description("ノードを展開する毎に子ノードを更新するかどうかを指定します。既定値はfalseです。"),Category("共通")]
        public bool IsAlwaysUpdateNode
        {
            get { return (bool)GetValue(IsAlwaysUpdateNodeProperty); }
            set { SetValue(IsAlwaysUpdateNodeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsAlwaysUpdateNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAlwaysUpdateNodeProperty =
            DependencyProperty.Register("IsAlwaysUpdateNode", typeof(bool), typeof(DirectoryNode), new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.Inherits));
        #endregion
        #region FolderErrorIconプロパティ
        [Description("ディレクトリが見つからなかった時のエラーアイコンです。"),Category("共通")]
        public ImageSource FolderErrorIcon
        {
            get { return (ImageSource)GetValue(FolderErrorIconProperty); }
            set { SetValue(FolderErrorIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ErrorIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderErrorIconProperty =
            DependencyProperty.Register("FolderErrorIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(Properties.Resources.FolderError_16x.ToImageSource(),FrameworkPropertyMetadataOptions.Inherits,OnIconChanged));
        #endregion
        #region FileIconプロパティ
        [Description("ファイルのアイコンです。設定しなければ関連付けられたアイコンを設定します。"), Category("共通")]
        public ImageSource FileIcon
        {
            get { return (ImageSource)GetValue(FileIconProperty); }
            set { SetValue(FileIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIconProperty =
            DependencyProperty.Register("FileIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnFileChanged));
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
            DependencyProperty.Register("FileErrorIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(Properties.Resources.FileError_16x.ToImageSource(),FrameworkPropertyMetadataOptions.Inherits, OnFileChanged));
        #endregion
        [Description("ディレクトリに関連付けられた展開時のアイコンです。実際は格納時と同じアイコンになります。(読取り専用)"), Category("共通")]
        public ImageSource AssociatedOpenIcon { get; private set; } = null;
        [Description("ディレクトリに関連付けられた格納時のアイコンです。実際は展開時と同じアイコンになります。(読取り専用)"), Category("共通")]
        public ImageSource AssociatedCloseIcon { get; private set; } = null;
        #endregion
        #region フィールド
        /// <summary>
        /// 1度でも展開したかどうか
        /// </summary>
        private bool hasExpandedOnce = false;
        #endregion
        #region メソッド
        public DirectoryNode()
        {
            this.UpdateDirectoryNode();
        }
        public DirectoryNode(string dirPath)
        {
            if (dirPath != null) this.Directory = new DirectoryInfo(dirPath);
            else this.UpdateDirectoryNode();
        }
        public DirectoryNode(DirectoryInfo dir)
        {
            this.Directory = dir;
        }
        /// <summary>
        /// DirectoryNodeを更新する。
        /// </summary>
        public void UpdateDirectoryNode()
        {
            if (this.Directory.IsNotNullOrExists())
            {
                this.HeaderText = this.Directory.Name;
                this.AddDummyNode();
                var icon = SHGetFileInfoEx.GetAssociatedImage(this.Directory.FullName, false).ToImageSource();
                this.AssociatedOpenIcon = this.OpenFolderIcon == null ? icon : null;
                this.AssociatedCloseIcon = this.CloseFolderIcon == null ? icon : null;
                this.UpdateHeaderIcon();
                return;
            }
            this.HeaderText = "フォルダが見つかりません";
            this.UpdateHeaderIcon();
        }
        /// <summary>
        /// ノードの展開状態に応じてヘッダーアイコンを変更する。
        /// ヘッダーアイコンが設定されていなければ(nullなら)パスに関連付けられたアイコンがをセットする。
        /// ディレクトリが存在しなければエラーアイコンをセットする。
        /// </summary>
        private void UpdateHeaderIcon()
        {
            ImageSource icon;
            if (this.Directory.IsNotNullOrExists())
            {
                if (this.IsExpanded) icon = this.OpenFolderIcon ?? this.AssociatedOpenIcon;
                else icon = this.CloseFolderIcon ?? this.AssociatedCloseIcon;
            }
            else icon = this.FolderErrorIcon;
            this.HeaderIcon = icon;
        }
        /// <summary>
        /// 全てのファイルノードのFileIconを変更する。
        /// </summary>
        private void UpdateFileIcon()
        {
            if (!this.HasItems) return;
            this.Items.Cast<object>().Where(x => x is FileNode).Cast<FileNode>().ToList().ForEach(x =>
            {
                x.FileIcon = this.FileIcon;
                x.FileErrorIcon = this.FileErrorIcon;
            });
        }
        /// <summary>
        /// 指定したディレクトリにサブディレクトリかファイルが存在するならダミーノードを追加する。
        /// </summary>
        /// <param name="dir"></param>
        private void AddDummyNode()
        {
            if (!this.Directory.Exists) return;
            try
            {
                if (this.Directory.EnumerateFileSystemInfos().Any()) this.AddChild(new BaseNode());
            }
            //アクセス拒否、ディレクトリ・ファイルが見つからないエラーをスキップ
            catch (Exception e) when (e is UnauthorizedAccessException || e is DirectoryNotFoundException || e is FileNotFoundException)
            {
                Console.WriteLine($"{e.Source}：{e.Message}");
            }
        }
        /// <summary>
        /// 自身のサブディレクトリとファイルを全て検索して子要素として追加する。
        /// </summary>
        private void AddNode()
        {
            this.Items.Clear();
            var directory = this.Directory as DirectoryInfo;
            if (directory == null) return;
            if (!directory.Exists) return;
            //DirectoryNodeを追加
            directory.GetDirectories().ToList().ForEach(x => this.AddChild(new DirectoryNode(x)));
            //パターンに一致するFileNodeを追加
            directory.GetFiles().Where(x => new Regex(this.SearchPattern).IsMatch(x.Name)).ToList().ForEach(x => this.AddChild(new FileNode(x) { FileIcon = this.FileIcon }));
        }
        #region コールバック
        /// <summary>
        /// Icon系プロパティ変更時、UpdateHeaderIconを実行する。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DirectoryNode;
            if (self == null) return;
            self.UpdateHeaderIcon();
        }
        /// <summary>
        /// File系プロパティ変更時、UpdateFileIconを実行する。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DirectoryNode;
            if (self == null) return;
            self.UpdateFileIcon();
        }
        /// <summary>
        /// Directoryに入るオブジェクトをDirectoryInfoに制限する(stringは変換する)。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceDirectory(DependencyObject d, object baseValue)
        {
            if (baseValue is string) return new DirectoryInfo((string)baseValue);
            else if (baseValue is DirectoryInfo) return baseValue;
            else throw new ArgumentException("Directoryに指定できるのはstring型のパスかDirectoryInfo型のみです。");
        }
        /// <summary>
        /// ディレクトリ変更時、UpdateDirectoryNodeを実行する。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnDirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DirectoryNode;
            self.UpdateDirectoryNode();
        }
        #endregion
        #region Override
        /// <summary>
        /// ノード展開時、ヘッダーアイコンを切り替える。
        /// また、はじめて展開した時は子ノードを探査して追加する。
        /// (IsAlwaysUpdateNodeプロパティがtrueなら常に初展開扱いになり、展開する度にノードが更新される)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExpanded(RoutedEventArgs e)
        {
            base.OnExpanded(e);
            this.UpdateHeaderIcon();
            if (!this.hasExpandedOnce)
            {
                this.AddNode();
                this.hasExpandedOnce = !IsAlwaysUpdateNode;
            }
        }
        /// <summary>
        /// ノード展開の格納時、ヘッダーアイコンを切り替える。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollapsed(RoutedEventArgs e)
        {
            base.OnCollapsed(e);
            this.UpdateHeaderIcon();
        }
        #endregion
        #endregion
    }
}
