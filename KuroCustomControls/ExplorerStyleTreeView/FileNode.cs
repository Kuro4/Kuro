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

namespace KuroCustomControls
{
    /// <summary>
    /// File用のNode
    /// </summary>
    public class FileNode : BaseNode
    {
        #region プロパティ
        #region FileIconプロパティ
        public ImageSource FileIcon
        {
            get { return (ImageSource)GetValue(FileIconProperty); }
            set { SetValue(FileIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FileIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIconProperty =
            DependencyProperty.Register("FileIcon", typeof(ImageSource), typeof(FileNode), new PropertyMetadata(Properties.Resources.Document_16x.ToImageSource(), OnIconChanged));
        #endregion
        #region Fileプロパティ
        [Description("このノードのファイルです。string型のパス、もしくはFileInfo型のインスタンスを指定します。"), Category("共通")]
        public object File
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
            DependencyProperty.Register("ShowFileExtension", typeof(bool), typeof(FileNode), new FrameworkPropertyMetadata(true,FrameworkPropertyMetadataOptions.Inherits, OnShowFileExtensionChanged));
        #endregion
        #endregion
        #region メソッド
        public FileNode()
        {
            this.HeaderIcon = this.FileIcon;
            this.UpdateFileNode();
        }
        public FileNode(FileInfo file)
        {
            this.File = file;
            this.HeaderIcon = this.FileIcon;
        }
        /// <summary>
        /// FileNodeを更新する
        /// </summary>
        public void UpdateFileNode()
        {
            var file = (FileInfo)this.File;
            if (file != null)
            {
                if (file.Exists)
                {
                    this.HeaderText = this.ShowFileExtension ? file.Name : file.Name.Replace(file.Extension, "");
                    this.FileIcon = IconUtility.GetFileAssociatedBitmap(file);
                    return;
                }
            }
            this.HeaderText = "ファイルが見つかりません";
            this.FileIcon = Properties.Resources.FileError_16x.ToImageSource();
        }
        #region コールバック
        /// <summary>
        /// Iconプロパティ変更時にヘッダーアイコンを更新する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as FileNode;
            if (self == null) return;
            self.HeaderIcon = (ImageSource)e.NewValue;
        }
        /// <summary>
        /// Fileに入るオブジェクトをFileInfoに制限する(stringは変換する)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceFile(DependencyObject d, object baseValue)
        {
            if (baseValue is string)
            {
                return new FileInfo((string)baseValue);
            }
            else if (baseValue is FileInfo)
            {
                return baseValue;
            }
            else
            {
                throw new ArgumentException("Fileに指定できるのはstring型のパスかFileInfo型のみです。");
            }
        }
        /// <summary>
        /// ファイル変更時、ノードを更新する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (FileNode)d;
            self.UpdateFileNode();
        }
        /// <summary>
        /// ShowFileExtension変更時、ノードを更新する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnShowFileExtensionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (FileNode)d;
            self.UpdateFileNode();
        }
        #endregion
        #endregion

    }
}
