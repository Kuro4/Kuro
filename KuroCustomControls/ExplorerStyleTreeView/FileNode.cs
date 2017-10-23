using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuroCustomControls
{
    /// <summary>
    /// File用のNode
    /// </summary>
    public class FileNode : BaseNode
    {
        public FileNode(FileInfo file, TreeViewHelper helper)
        {
            this._Info = file;
            this._HeaderImage.Source = helper._FileIcon;
            this._HeaderImage.Width = helper._ImageWidth;
            this._HeaderImage.Height = helper._ImageHeight;
            this._HeaderText.Text = file.Name;
        }
    }
}
