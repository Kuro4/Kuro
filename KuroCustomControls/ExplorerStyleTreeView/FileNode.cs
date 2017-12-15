using KuroUtilities.Icon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using KuroUtilities.Win32;
using KuroUtilities.FileSystemInfo;

namespace KuroCustomControls
{
    /// <summary>
    /// File用のNode
    /// </summary>
    public class FileNode : BaseNode
    {
        #region プロパティ
        #region FileIconプロパティ
        [Description("ファイルのアイコンです。設定しなければ関連付けられたアイコンを設定します。"),Category("共通")]
        public ImageSource FileIcon
        {
            get { return (ImageSource)GetValue(FileIconProperty); }
            set { SetValue(FileIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIconProperty =
            DependencyProperty.Register("FileIcon", typeof(ImageSource), typeof(FileNode), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.Inherits, OnIconChanged));
        #endregion
        #region Fileプロパティ
        [Description("このノードのファイルです。string型のパス、もしくはFileInfo型のインスタンスを指定します。"), Category("共通")]
        public FileInfo File
        {
            get { return (FileInfo)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }
        // Using a DependencyProperty as the backing store for File.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(object), typeof(FileNode), new PropertyMetadata(null, OnFileChanged, OnCoerceFile));
        #endregion
        #region ShowFileExtension
        [Description("ファイルの拡張子を表示するかを設定します。既定値はtrueです。"),Category("共通")]
        public bool ShowFileExtension
        {
            get { return (bool)GetValue(ShowFileExtensionProperty); }
            set { SetValue(ShowFileExtensionProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ShowFileExtension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowFileExtensionProperty =
            DependencyProperty.Register("ShowFileExtension", typeof(bool), typeof(FileNode), new FrameworkPropertyMetadata(true,FrameworkPropertyMetadataOptions.Inherits, OnFileChanged));
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
            DependencyProperty.Register("FileErrorIcon", typeof(ImageSource), typeof(FileNode), new FrameworkPropertyMetadata(Properties.Resources.FileError_16x.ToImageSource(),FrameworkPropertyMetadataOptions.Inherits, OnIconChanged));
        #endregion
        [Description("ファイルに関連付けられたアイコンです。FileIconを設定するとnullになります。(読取り専用)"), Category("共通")]
        public ImageSource AssociatedFileIcon { get; private set; } = null;
        #endregion
        #region メソッド
        public FileNode()
        {
            this.UpdateFileNode();
        }
        public FileNode(string filePath)
        {
            if (filePath != null) this.File = new FileInfo(filePath);
            else this.UpdateFileNode();
        }
        public FileNode(FileInfo file)
        {
            this.File = file;
        }
        /// <summary>
        /// FileNodeを更新する。
        /// </summary>
        public void UpdateFileNode()
        {
            if (this.File.IsNotNullOrExists())
            {
                this.HeaderText = this.ShowFileExtension ? this.File.Name : this.File.Name.Replace(this.File.Extension, "");
                this.AssociatedFileIcon = this.FileIcon == null ? SHGetFileInfoEx.GetAssociatedImage(this.File.FullName, false).ToImageSource() : null;
            }
            else this.HeaderText = "ファイルが見つかりません";
            this.UpdateHeaderIcon();
        }
        /// <summary>
        /// ヘッダーアイコンを更新する。
        /// FileIconが設定されていなければ(nullなら)パスに関連付けられたアイコンをセットする。
        /// ファイルが存在しなければエラーアイコンをセットする。
        /// </summary>
        private void UpdateHeaderIcon()
        {
            if (this.File.IsNotNullOrExists()) this.HeaderIcon = this.FileIcon ?? this.AssociatedFileIcon;
            else this.HeaderIcon = this.FileErrorIcon;
        }
        #region コールバック
        /// <summary>
        /// Iconプロパティ変更時にヘッダーアイコンを更新する。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as FileNode;
            if (self == null) return;
            self.UpdateHeaderIcon();
        }
        /// <summary>
        /// Fileに入るオブジェクトをFileInfoに制限する(stringは変換する)。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceFile(DependencyObject d, object baseValue)
        {
            if (baseValue is string) return new FileInfo((string)baseValue);
            else if (baseValue is FileInfo) return baseValue;
            else throw new ArgumentException("Fileに指定できるのはstring型のパスかFileInfo型のみです。");
        }
        /// <summary>
        /// ファイル変更時、ノードを更新する。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (FileNode)d;
            self.UpdateFileNode();
        }
        #endregion
        #endregion
    }
}
