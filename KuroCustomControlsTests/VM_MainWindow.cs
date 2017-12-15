using KuroCustomControls;
using KuroUtilities.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace KuroCustomControlsTests
{
    public class VM_MainWindow
    {
        public ObservableCollection<string> _Roots { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<DirectoryNode> _ObRoots { get; set; } = new ObservableCollection<DirectoryNode>();

        public VM_MainWindow()
        {
            _Roots.Add("test1");
            _Roots.Add(@"test2");

            _ObRoots.Add(new DirectoryNode(@"C:\M.EYEチェッカー"));
            _ObRoots.Add(new DirectoryNode(@"C:\新しいフォルダー"));
        }
    }
}
