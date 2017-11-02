﻿using KuroUtilities.Icon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ExplorerStyleTreeView : TreeView
    {
        #region プロパティ
        [Description("ルートフォルダのコレクションです。"), Category("共通")]
        public ObservableCollection<DirectoryInfo> Roots
        {
            get { return (ObservableCollection<DirectoryInfo>)GetValue(RootsProperty); }
            set { SetValue(RootsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Roots.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RootsProperty =
            DependencyProperty.Register("Roots", typeof(int), typeof(ExplorerStyleTreeView), new PropertyMetadata(null));
        #region FolderImageプロパティ
        [Description("フォルダのアイコンです。設定しなければ標準のアイコンになります。"), Category("共通")]
        public ImageSource FolderIcon
        {
            get { return (ImageSource)GetValue(FolderIconProperty); }
            set { SetValue(FolderIconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for FolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderIconProperty =
            DependencyProperty.Register("FolderIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new PropertyMetadata(null));
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
            DependencyProperty.Register("FileIcon", typeof(ImageSource), typeof(ExplorerStyleTreeView), new PropertyMetadata(null));
        #endregion





        #endregion
        #region フィールド

        #endregion
        #region メソッド
        public ExplorerStyleTreeView()
        {
            SetDefaultDirectories();
        }

        private void SetDefaultDirectories()
        {
            foreach (var drive in DriveInfo.GetDrives().Where(x => x.IsReady))
            {
                this.AddChild(new DirectoryNode(drive.RootDirectory));
            }
            this.AddChild(new DirectoryNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            var dirNode = new DirectoryNode(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            dirNode.OpenFolderIcon = Properties.Resources.FolderOpen_16x.ToImageSource();
            dirNode.CloseFolderIcon = Properties.Resources.Folder_16x.ToImageSource();
            this.AddChild(dirNode);
            #endregion
        }
    }
}
