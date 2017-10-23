using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KuroCustomControls
{
    /// <summary>
    /// ルートディレクトリ用クラス
    /// <para>_RootDirectoryに追加したNodeがルートになる</para>
    /// <para>コンストラクタでは1つのルートだけ追加するか、コレクションで複数のルートを追加できる</para>
    /// </summary>
    public class RootDirectoryNode
    {
        /// <summary>
        /// ルートNodeのリスト
        /// こいつをバインドする
        /// </summary>
        public ObservableCollection<BaseNode> _RootDirectory { get; private set; } = new ObservableCollection<BaseNode>();

        /// <summary>
        /// 1つのルートノードを持たせるコンストラクタ(string)
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="helper"></param>
        public RootDirectoryNode(string rootDirectoryPath, TreeViewHelper helper)
        {
            _RootDirectory.Add(TryCreateDirectoryNode(rootDirectoryPath, helper));
        }

        /// <summary>
        /// 1つのルートノードを持たせるコンストラクタ(DirectoryInfo)
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="helper"></param>
        public RootDirectoryNode(DirectoryInfo rootDirectory, TreeViewHelper helper)
        {
            _RootDirectory.Add(new DirectoryNode(rootDirectory, helper));
        }

        /// <summary>
        /// 複数のルートノードを持たせるコンストラクタ(string)
        /// </summary>
        /// <param name="rootDirectoryPathList"></param>
        /// <param name="helper"></param>
        public RootDirectoryNode(IEnumerable<string> rootDirectoryPathList, TreeViewHelper helper)
        {
            CommonSetRootDirectories(rootDirectoryPathList.Select(x => TryCreateDirectoryNode(x, helper)).ToList());
        }

        /// <summary>
        /// 複数のルートノードを持たせるコンストラクタ(DirectoryInfo)
        /// </summary>
        /// <param name="rootDirectoryPathList"></param>
        /// <param name="helper"></param>
        public RootDirectoryNode(IEnumerable<DirectoryInfo> rootDirectoryList, TreeViewHelper helper)
        {
            CommonSetRootDirectories(rootDirectoryList.Select(x => new DirectoryNode(x, helper)).ToList());
        }

        /// <summary>
        /// 1つのルートノードを再度セットする(string)
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="helper"></param>
        public void SetRootDirectory(string rootDirectoryPath, TreeViewHelper helper)
        {
            CommonSetRootDirectory(TryCreateDirectoryNode(rootDirectoryPath, helper));
        }

        /// <summary>
        /// 1つのルートノードを再度セットする(DirectoryInfo)
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="helper"></param>
        public void SetRootDirectory(DirectoryInfo rootDirectory, TreeViewHelper helper)
        {
            CommonSetRootDirectory(new DirectoryNode(rootDirectory, helper));
        }

        /// <summary>
        /// 複数のルートノードを再度セットする(string)
        /// </summary>
        /// <param name="rootDirectoryPaths"></param>
        /// <param name="helper"></param>
        public void SetRootDirectories(IEnumerable<string> rootDirectoryPaths, TreeViewHelper helper)
        {
            CommonSetRootDirectories(rootDirectoryPaths.Select(x => TryCreateDirectoryNode(x, helper)).ToList());
        }

        /// <summary>
        /// 複数のルートノードを再度セットする(DirectoryInfo)
        /// </summary>
        /// <param name="rootDirectoryPathList"></param>
        /// <param name="helper"></param>
        public void SetRootDirectories(IEnumerable<DirectoryInfo> rootDirectories, TreeViewHelper helper)
        {
            CommonSetRootDirectories(rootDirectories.Select(x => new DirectoryNode(x, helper)).ToList());
        }

        /// <summary>
        /// ルートノードに指定パスのDirectoryNodeを追加する
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="helper"></param>
        public void AddRootDirectory(string rootDirectoryPath, TreeViewHelper helper)
        {
            _RootDirectory.Add(TryCreateDirectoryNode(rootDirectoryPath, helper));
        }

        /// <summary>
        /// ルートノードに指定ディレクトリのDirectoryNodeを追加する
        /// </summary>
        /// <param name="rootDirectoryPath"></param>
        /// <param name="helper"></param>
        public void AddRootDirectory(DirectoryInfo rootDirectoryPath, TreeViewHelper helper)
        {
            _RootDirectory.Add(new DirectoryNode(rootDirectoryPath, helper));
        }

        /// <summary>
        /// ルートノードに指定パスのFileNodeを追加する
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="helper"></param>
        public void AddFileNode(string filePath, TreeViewHelper helper)
        {
            _RootDirectory.Add(TryCreateFileNode(filePath, helper));
        }

        /// <summary>
        /// ルートノードに指定ファイルのFileNodeを追加する
        /// </summary>
        /// <param name="file"></param>
        /// <param name="helper"></param>
        public void AddFileNode(FileInfo file, TreeViewHelper helper)
        {
            _RootDirectory.Add(new FileNode(file, helper));
        }

        /// <summary>
        /// ルートノードを一旦クリアし、引数のノードを追加する
        /// </summary>
        /// <param name="dirNode"></param>
        private void CommonSetRootDirectory(BaseNode dirNode)
        {
            _RootDirectory.Clear();
            _RootDirectory.Add(dirNode);
        }

        /// <summary>
        /// ルートノードを一旦クリアし、引数のノード全てを追加する
        /// </summary>
        /// <param name="dirNodes"></param>
        private void CommonSetRootDirectories(IEnumerable<BaseNode> dirNodes)
        {
            _RootDirectory.Clear();
            foreach (var dirNode in dirNodes)
            {
                _RootDirectory.Add(dirNode);
            }
        }

        /// <summary>
        /// 指定パスにディレクトリが存在するならDirectoryNodeを返す
        /// <para>存在しなければBaseNodeを返す</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="helper"></param>
        /// <returns></returns>
        private BaseNode TryCreateDirectoryNode(string path, TreeViewHelper helper)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryNode(new DirectoryInfo(path), helper);
            }
            else
            {
                return new BaseNode() { Header = "フォルダが見つかりません" };
            }
        }

        /// <summary>
        /// 指定パスにファイルが存在するならDirectoryNodeを返す
        /// <para>存在しなければBaseNodeを返す</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="helper"></param>
        /// <returns></returns>
        private BaseNode TryCreateFileNode(string path, TreeViewHelper helper)
        {
            if (File.Exists(path))
            {
                return new FileNode(new FileInfo(path), helper);
            }
            else
            {
                return new BaseNode() { Header = "ファイルが見つかりません" };
            }
        }
    }

    /// <summary>
    /// 各情報を参照型として共有するためのヘルパークラス
    /// </summary>
    public class TreeViewHelper
    {
        /// <summary>
        /// ファイル検索時のフィルター
        /// </summary>
        public string _SearchPattern { get; set; } = "*";
        /// <summary>
        /// Headerに表示するアイコンの幅
        /// </summary>
        public double _ImageWidth { get; set; } = 18;
        /// <summary>
        /// Headerに表示するアイコンの高さ
        /// </summary>
        public double _ImageHeight { get; set; } = 15;
        /// <summary>
        /// フォルダ未展開時にHeaderに表示するアイコン
        /// </summary>
        public ImageSource _CloseFolderIcon { get; set; } = new BitmapImage();
        /// <summary>
        /// フォルダ展開時にHeaderに表示するアイコン
        /// </summary>
        public ImageSource _OpenFolderIcon { get; set; } = new BitmapImage();
        /// <summary>
        /// ファイルのHeaderに表示するアイコン
        /// </summary>
        public ImageSource _FileIcon { get; set; } = new BitmapImage();

        public TreeViewHelper(ImageSource closeFolderIcon, ImageSource openFolderIcon, ImageSource fileIcon, string searchPattern = "*", double imageWidth = 18, double imageHeight = 15)
        {
            _SearchPattern = searchPattern;
            _ImageWidth = imageWidth;
            _ImageHeight = imageHeight;
            _CloseFolderIcon = closeFolderIcon;
            _OpenFolderIcon = openFolderIcon;
            _FileIcon = fileIcon;
        }
    }
}