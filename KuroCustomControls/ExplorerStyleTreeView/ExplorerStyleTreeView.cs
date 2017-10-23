using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        #endregion
        #region フィールド

        #endregion
        #region メソッド
        ///// <summary>
        ///// イベントの登録・解除
        ///// </summary>
        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    // 前のテンプレートのコントロールの後処理(イベントハンドラの解除)
        //    if (this.upButton != null)
        //    {
        //        this.upButton.Click -= this.UpClick;
        //    }
        //    // テンプレートからコントロールの取得
        //    this.valueBox = this.GetTemplateChild("PART_ValueBox") as TextBox;
        //    // イベントハンドラの登録
        //    if (this.upButton != null)
        //    {
        //        this.upButton.Click += this.UpClick;
        //    }
        //}
        #endregion
    }
}
