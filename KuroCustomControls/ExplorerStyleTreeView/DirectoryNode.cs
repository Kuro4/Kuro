using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KuroCustomControls
{
    /// <summary>
    /// Directory用のNode
    /// </summary>
    public class DirectoryNode : BaseNode
    {
        /// <summary>
        /// 共有のためのTreeViewHelper
        /// </summary>
        public TreeViewHelper Helper;
        /// <summary>
        /// 1度でも展開したかどうか
        /// </summary>
        private bool hasExpandedOnce = false;

        /// <summary>
        /// 自身のDirectory内にサブDirectoryか_SearchPatternに一致するファイルがあれば、
        /// 展開できることを表示するためにダミーノードを追加する
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="helper"></param>
        public DirectoryNode(DirectoryInfo dir, TreeViewHelper helper)
        {
            this._Info = dir;
            this.Helper = helper;
            this._HeaderImage.Source = Helper._CloseFolderIcon;
            this._HeaderImage.Width = Helper._ImageWidth;
            this._HeaderImage.Height = Helper._ImageHeight;
            this._HeaderText.Text = dir.Name;

            if (dir.Exists)
            {
                try
                {
                    if (dir.EnumerateDirectories().Any() || dir.EnumerateFiles(Helper._SearchPattern).Any())
                    {
                        this.Items.Add(new BaseNode());//ダミーノードの追加
                    }
                }
                //アクセス拒否、ディレクトリ・ファイルが見つからないエラーをスキップ
                catch (Exception e) when (e is UnauthorizedAccessException || e is DirectoryNotFoundException || e is FileNotFoundException)
                {
                    Console.WriteLine(e.Source + "：" + e.Message);
                }
            }
            this.Expanded += DirNode_Expanded;
            this.Collapsed += DirNode_Collapsed;
        }

        /// <summary>
        /// 展開した時、子ノードがあれば_HeaderImageを_OpenFolderIconに変更する
        /// また、はじめて展開した時ならサブDirectoryとFileを探査して子ノードに追加する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirNode_Expanded(object sender, RoutedEventArgs e)
        {
            var directory = (DirectoryInfo)this._Info;

            if (this.Items.Count > 0)
            {
                this._HeaderImage.Source = this.Helper._OpenFolderIcon;
                if (!hasExpandedOnce)
                {
                    this.Items.Clear();
                    foreach (var dir in directory.GetDirectories())
                    {
                        this.Items.Add(new DirectoryNode(dir, Helper));
                    }
                    foreach (var file in directory.GetFiles(Helper._SearchPattern))
                    {
                        this.Items.Add(new FileNode(file, Helper));
                    }
                    hasExpandedOnce = true;
                }
            }
        }

        /// <summary>
        /// 展開を閉じた時、_HeaderImageを_CloseFolderIconに変更する
        /// このイベントは親Nodeまで伝播するのでIsExpandedプロパティで判定する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirNode_Collapsed(object sender, RoutedEventArgs e)
        {
            if (!this.IsExpanded) this._HeaderImage.Source = this.Helper._CloseFolderIcon;
        }
    }
}
