using KuroUtilities.Icon;
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
        #region OpenFolderIcon
        [Description("ノード展開時に表示するフォルダのアイコンを指定します。"),Category("共通")]
        public ImageSource OpenFolderIcon
        {
            get { return (ImageSource)GetValue(OpenFolderIconProperty); }
            set { SetValue(OpenFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for OpenFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenFolderIconProperty =
            DependencyProperty.RegisterAttached("OpenFolderIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(Properties.Resources.FolderOpen_16x.ToImageSource(),FrameworkPropertyMetadataOptions.Inherits,OnIconChanged));
        #endregion
        #region CloseFolderIcon
        [Description("ノード格納時に表示するフォルダのアイコンを指定します。"), Category("共通")]
        public ImageSource CloseFolderIcon
        {
            get { return (ImageSource)GetValue(CloseFolderIconProperty); }
            set { SetValue(CloseFolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CloseFolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseFolderIconProperty =
            DependencyProperty.RegisterAttached("CloseFolderIcon", typeof(ImageSource), typeof(DirectoryNode), new FrameworkPropertyMetadata(Properties.Resources.Folder_16x.ToImageSource(),FrameworkPropertyMetadataOptions.Inherits, OnIconChanged));
        #endregion
        #region Directoryプロパティ
        [Description("このノードのディレクトリです。string型のパス、もしくはDirectoryInfo型のインスタンスを指定します。"),Category("共通")]
        public object Directory
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
            DependencyProperty.Register("SearchPattern", typeof(string), typeof(DirectoryNode), new PropertyMetadata(@"..*"));
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
            this.HeaderIcon= this.CloseFolderIcon;
            this.UpdateDirectoryNode();
        }
        public DirectoryNode(string dirPath)
        {
            this.HeaderIcon = this.CloseFolderIcon;
            if (dirPath != null) this.Directory = new DirectoryInfo(dirPath);
            else this.UpdateDirectoryNode();
        }
        public DirectoryNode(DirectoryInfo dir)
        {
            this.HeaderIcon = this.CloseFolderIcon;
            this.Directory = dir;
        }

        /// <summary>
        /// DirectoryNodeを更新する
        /// </summary>
        public void UpdateDirectoryNode()
        {
            var dir = (DirectoryInfo)this.Directory;
            if (dir != null)
            {
                if (dir.Exists)
                {
                    this.HeaderText = dir.Name;
                    this.AddDummyNode(dir);
                    this.OpenFolderIcon = (ImageSource)OpenFolderIconProperty.DefaultMetadata.DefaultValue;
                    this.CloseFolderIcon = (ImageSource)CloseFolderIconProperty.DefaultMetadata.DefaultValue;
                    return;
                }
            }
            this.HeaderText = "フォルダが見つかりません";
            var errIcon = Properties.Resources.FolderError_16x.ToImageSource();
            this.OpenFolderIcon = errIcon;
            this.CloseFolderIcon = errIcon;
        }
        /// <summary>
        /// 指定したディレクトリにサブディレクトリかファイルが存在するならダミーノードを追加する
        /// </summary>
        /// <param name="dir"></param>
        private void AddDummyNode(DirectoryInfo dir)
        {
            if (!dir.Exists) return;
            try
            {
                //サブディレクトリ・ファイルの存在を検索
                if (dir.EnumerateFileSystemInfos().Any())
                {
                    this.AddChild(new BaseNode());
                }
            }
            //アクセス拒否、ディレクトリ・ファイルが見つからないエラーをスキップ
            catch (Exception e) when (e is UnauthorizedAccessException || e is DirectoryNotFoundException || e is FileNotFoundException)
            {
                Console.WriteLine(e.Source + "：" + e.Message);
            }
        }
        /// <summary>
        /// 自身のサブディレクトリとファイルを全て検索して子要素として追加する
        /// </summary>
        private void AddNode()
        {
            this.Items.Clear();
            var directory = this.Directory as DirectoryInfo;
            if (directory == null) return;
            //DirectoryNodeを追加
            directory.GetDirectories().ToList().ForEach(x => this.AddChild(new DirectoryNode(x)));
            //パターンに一致するFileNodeを追加
            directory.GetFiles().Where(x => new Regex(this.SearchPattern).IsMatch(x.Name)).ToList().ForEach(x => this.AddChild(new FileNode(x)));
        }
        #region コールバック
        /// <summary>
        /// Iconプロパティ変更時にヘッダーアイコンを更新する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DirectoryNode;
            if (self == null) return;
            self.HeaderIcon = self.IsExpanded ? self.OpenFolderIcon : self.CloseFolderIcon;
        }
        /// <summary>
        /// Directoryに入るオブジェクトをDirectoryInfoに制限する(stringは変換する)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceDirectory(DependencyObject d, object baseValue)
        {
            if (baseValue is string)
            {
                return new DirectoryInfo((string)baseValue);
            }
            else if (baseValue is DirectoryInfo)
            {
                return baseValue;
            }
            else
            {
                throw new ArgumentException("Directoryに指定できるのはstring型のパスかDirectoryInfo型のみです。");
            }
        }
        /// <summary>
        /// ディレクトリ変更時にヘッダーテキストを変更し、子ノードがあれば追加する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnDirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (DirectoryNode)d;
            self.UpdateDirectoryNode();
        }
        #endregion
        #region Override
        /// <summary>
        /// ノード展開時、ヘッダーアイコンを切り替える
        /// また、はじめて展開した時は子ノードを探査して追加する
        /// (IsAlwaysUpdateNodeプロパティがtrueなら常に初展開扱いになり、展開する度にノードが更新される)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExpanded(RoutedEventArgs e)
        {
            base.OnExpanded(e);
            this.HeaderIcon = this.OpenFolderIcon;
            if (!this.hasExpandedOnce)
            {
                this.AddNode();
                this.hasExpandedOnce = !IsAlwaysUpdateNode;
            }
        }
        /// <summary>
        /// ノード展開の格納時、ヘッダーアイコンを切り替える
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollapsed(RoutedEventArgs e)
        {
            base.OnCollapsed(e);
            this.HeaderIcon = this.CloseFolderIcon;
        }
        #endregion
        #endregion
    }
}
