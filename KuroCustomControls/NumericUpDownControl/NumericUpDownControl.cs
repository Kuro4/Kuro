using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace KuroCustomControls
{
    public class NumericUpDownControl : Control
    {
        static NumericUpDownControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDownControl),
                new FrameworkPropertyMetadata(typeof(NumericUpDownControl)));
        }
        #region プロパティ
        #region Textプロパティ
        [Description("TextBoxに表示するテキストです。"), Category("共通"),Browsable(false),ReadOnly(true)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDownControl), new PropertyMetadata("0"));
        #endregion

        #region Valueプロパティ
        [Description("TextBoxに表示する値です。"), Category("共通")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDownControl), new PropertyMetadata((double)0, ValueChanged));
        #endregion
        #region UpContentプロパティ
        [Description("Valueを加算するボタンに表示するContentです。"), Category("共通")]
        public object UpContent
        {
            get { return (object)GetValue(UpContentProperty); }
            set { SetValue(UpContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpContentProperty =
            DependencyProperty.Register("UpContent", typeof(object), typeof(NumericUpDownControl), new PropertyMetadata("↑"));
        #endregion
        #region DownContentプロパティ
        [Description("Valueを減算するボタンに表示するContentです。"), Category("共通")]
        public object DownContent
        {
            get { return (object)GetValue(DownContentProperty); }
            set { SetValue(DownContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownContentProperty =
            DependencyProperty.Register("DownContent", typeof(object), typeof(NumericUpDownControl), new PropertyMetadata("↓"));
        #endregion
        #region TextBoxWidthプロパティ
        [Description("TextBoxの幅です。"), Category("レイアウト")]
        public GridLength TextBoxWidth
        {
            get { return (GridLength)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextBoxWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register("TextBoxWidth", typeof(GridLength), typeof(NumericUpDownControl), new PropertyMetadata(new GridLength(3, GridUnitType.Star)));
        #endregion
        #region ButtonWidthプロパティ
        [Description("Buttonの幅です。"), Category("レイアウト")]
        public GridLength ButtonWidth
        {
            get { return (GridLength)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(GridLength), typeof(NumericUpDownControl), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));
        #endregion
        #region HorizontalValueAlignmentプロパティ
        [Description("TextBoxに表示する数値の水平方向の位置です。"), Category("レイアウト")]
        public HorizontalAlignment HorizontalValueAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalValueAlignmentProperty); }
            set { SetValue(HorizontalValueAlignmentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HorizontalValueAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalValueAlignmentProperty =
            DependencyProperty.Register("HorizontalValueAlignment", typeof(HorizontalAlignment), typeof(NumericUpDownControl), new PropertyMetadata(HorizontalAlignment.Center));
        #endregion
        #region MaxValueプロパティ
        [Description("Valueの上限値です。"), Category("共通")]
        public double? MaxValue
        {
            get { return (double?)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double?), typeof(NumericUpDownControl), new PropertyMetadata(null, EnsureMaxMinValueValidity));
        #endregion
        #region MinValueプロパティ
        [Description("Valueの下限値です。"), Category("共通")]
        public double? MinValue
        {
            get { return (double?)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double?), typeof(NumericUpDownControl), new PropertyMetadata(null, EnsureMaxMinValueValidity));
        #endregion
        #region Stepプロパティ
        [Description("Valueを増減させる時の変化値です。"), Category("共通")]
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(NumericUpDownControl), new PropertyMetadata((double)1));
        #endregion
        #region IsLoopプロパティ
        [Description("MaxValue,MinValueで指定した限界値を超えてValueが変化する時に値がループするかを設定します。(MaxValue,MinValueのどちらかを設定していなければループ時は0になります。)"), Category("共通")]
        public bool IsLoop
        {
            get { return (bool)GetValue(IsLoopProperty); }
            set { SetValue(IsLoopProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsLoop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoopProperty =
            DependencyProperty.Register("IsLoop", typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(false));
        #endregion
        #region IsReadOnlyプロパティ
        [Description("TextBoxの値を読み取り専用にするかどうかを設定します。"), Category("共通")]
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(true));
        #endregion
        #region IsHighlightToNegativeValueプロパティ
        [Description("Valueが負の値になる時に文字色を変更するかどうかを設定します。"), Category("共通")]
        public bool IsHighlightToNegativeValue
        {
            get { return (bool)GetValue(IsHighlightToNegativeValueProperty); }
            set { SetValue(IsHighlightToNegativeValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsHighlightToNegativeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHighlightToNegativeValueProperty =
            DependencyProperty.Register("IsHighlightToNegativeValue", typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(false, ValueChanged));
        #endregion
        #region DefaultValueColorプロパティ
        [Description("既定のTextBoxの文字色です。"), Category("共通")]
        public Color DefaultValueColor
        {
            get { return (Color)GetValue(DefaultValueColorProperty); }
            set { SetValue(DefaultValueColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DefaultForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultValueColorProperty =
            DependencyProperty.Register("DefaultValueColor", typeof(Color), typeof(NumericUpDownControl), new PropertyMetadata(Colors.Black, ColorChanged));
        #endregion
        #region NegativeValueColorプロパティ
        [Description("Valueが負の値になった時の文字色です。IsHighlightToNegativeValueがtrueの時のみ有効です。"), Category("共通")]
        public Color NegativeValueColor
        {
            get { return (Color)GetValue(NegativeValueColorProperty); }
            set { SetValue(NegativeValueColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HighlightBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NegativeValueColorProperty =
            DependencyProperty.Register("NegativeValueColor", typeof(Color), typeof(NumericUpDownControl), new PropertyMetadata(Colors.Red, ColorChanged));
        #endregion
        #region CurrentValueColorプロパティ
        [Description("現在のValueの色です。(読取専用)"), Category("共通")]
        private static readonly DependencyPropertyKey CurrentValueColorPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "CurrentValueColor",
                typeof(SolidColorBrush),
                typeof(NumericUpDownControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static readonly DependencyProperty CurrentValueColorProperty = CurrentValueColorPropertyKey.DependencyProperty;
        public SolidColorBrush CurrentValueColor
        {
            get { return (SolidColorBrush)GetValue(CurrentValueColorProperty); }
            private set { this.SetValue(CurrentValueColorPropertyKey, value); }
        }
        #endregion
        #endregion

        #region フィールド
        // XAMLで名前を付けた(イベントを使用する)コントロールの格納用変数
        private TextBox valueBox;
        private RepeatButton upButton;
        private RepeatButton downButton;
        #endregion

        /// <summary>
        /// イベントの登録・解除
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 前のテンプレートのコントロールの後処理(イベントハンドラの解除)
            if (this.upButton != null)
            {
                this.upButton.Click -= this.UpClick;
            }
            if (this.downButton != null)
            {
                this.downButton.Click -= this.DownClick;
            }
            if(this.valueBox != null)
            {
                this.valueBox.PreviewTextInput -= ValueBox_PreviewTextInput;
                CommandManager.RemoveExecutedHandler(this.valueBox, ValueBox_PreviewExecuted);
            }
            // テンプレートからコントロールの取得
            this.valueBox = this.GetTemplateChild("PART_ValueBox") as TextBox;
            this.upButton = this.GetTemplateChild("PART_UpButton") as RepeatButton;
            this.downButton = this.GetTemplateChild("PART_DownButton") as RepeatButton;

            // イベントハンドラの登録
            if (this.upButton != null)
            {
                this.upButton.Click += this.UpClick;
            }
            if (this.downButton != null)
            {
                this.downButton.Click += this.DownClick;
            }
            if(this.valueBox != null)
            {
                this.valueBox.PreviewTextInput += ValueBox_PreviewTextInput;
                //this.valueBox
                CommandManager.AddPreviewExecutedHandler(this.valueBox, ValueBox_PreviewExecuted);
            }
        }

        #region メソッド

        /// <summary>
        /// 現在のValueの色を更新する
        /// </summary>
        private void UpdateValueColor()
        {
            Color res;
            if (this.IsHighlightToNegativeValue)
            {
                if (this.Value >= 0) res = this.DefaultValueColor;
                else res = this.NegativeValueColor;
            }
            else res = this.DefaultValueColor;
            if (this.CurrentValueColor.Color != res) this.CurrentValueColor = new SolidColorBrush(res);
        }

        #region Valueの検証・矯正
        /// <summary>
        /// 引数の値が上下限値を超えているか検証し、超えていれば矯正した値を返す
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>矯正した値</returns>
        private bool ValidateValue(ref double value)
        {
            var resMax = value;
            var resMin = value;
            //上限値を超えていれば
            if (ValidateMaxValue(ref resMax)) value = resMax;
            //下限値を超えていれば
            else if (ValidateMinValue(ref resMin)) value = resMin;
            //どちらも超えていなければ(矯正していなければ)
            else return false;
            //矯正していれば
            return true;
        }
        /// <summary>
        /// 引数の値が上下限値を超えているか検証し、超えていれば矯正した値を返す
        /// 第2引数で上限値を検証するかを指定する(fakseなら下限値の検証)
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <param name="isMaxMode">上限値を検証するか</param>
        /// <returns>矯正した値</returns>
        private double ValidateValue(double value, bool isMaxMode)
        {
            if (isMaxMode) ValidateMaxValue(ref value);
            else ValidateMinValue(ref value);
            return value;
        }
        /// <summary>
        /// 上限値が設定されていれば上限値を超えているかを判定し、
        /// さらにループ設定されていれば下限値もしくは0を参照引数へ代入する
        /// また、値を矯正した場合はtrueを返す
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>値を矯正したかどうか</returns>
        private bool ValidateMaxValue(ref double value)
        {
            if (MaxValue.HasValue)
            {
                if (MaxValue.Value < value)
                {
                    if (IsLoop)
                    {
                        value = MinValue.HasValue ? MinValue.Value : 0;
                    }
                    else
                    {
                        value = MaxValue.Value;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 下限値が設定されていれば下限値を超えているかを判定し、
        /// さらにループ設定されていれば上限値もしくは0を参照引数へ代入する
        /// また、値を矯正した場合はtrueを返す
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>値を矯正したかどうか</returns>
        private bool ValidateMinValue(ref double value)
        {
            if (MinValue.HasValue)
            {
                if (MinValue.Value > value)
                {
                    if (IsLoop)
                    {
                        value = MaxValue.HasValue ? MaxValue.Value : 0;
                    }
                    else
                    {
                        value = MinValue.Value;
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region コールバック
        /// <summary>
        /// Value変更時のコールバックメソッド
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            var value = self.Value;
            var res = self.ValidateValue(ref value);
            self.Value = value;
            self.UpdateValueColor();
        }

        private static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            self.UpdateValueColor();
        }

        /// <summary>
        /// 上限値より下限値が大きくなることを禁止し、上限・下限値の妥当性を保証する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void EnsureMaxMinValueValidity(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            if (self.MaxValue.HasValue && self.MinValue.HasValue)
            {
                if (self.MaxValue.Value < self.MinValue.Value)
                {
                    if (e.Property.Name == "MaxValue")
                    {
                        self.MaxValue = self.MinValue;
                    }
                    else if (e.Property.Name == "MinValue")
                    {
                        self.MinValue = self.MaxValue;
                    }
                }
            }
        }
        #endregion
        #region イベント

        // ボタンのクリックイベント
        private void UpClick(object sender, RoutedEventArgs e) { this.Value += Step; }
        private void DownClick(object sender, RoutedEventArgs e) { this.Value -= Step; }

        /// <summary>
        /// 数値、小数点、-以外の入力を禁止する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)e.Source;
            var text = textBox.Text;
            //小数点は1つだけ許容する
            if (e.Text == ".") e.Handled = text.Contains(".");
            //-は先頭に1つだけ許容する(SelectedText内にあり、上書きする時も許容)
            else if (e.Text == "-")
            {
                if (text.Contains("-") && !textBox.SelectedText.Contains("-")) e.Handled = true;
                else e.Handled = textBox.CaretIndex != 0;
            }
            //-より前の入力を禁止する
            else if (text.Contains("-") && textBox.CaretIndex == 0 && textBox.SelectedText != text)
            {
                e.Handled = true;
            }
            //数字のみ許容する
            else e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        ///// <summary>
        ///// 入力された値を検証・矯正する
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ValueBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var textBox = (TextBox)e.Source;
        //    var text = textBox.Text;
        //    //空白なら0もしくは下限値を入れるを入れる
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        if (MinValue.HasValue) text = MinValue.Value.ToString();
        //        else text = "0";
        //        textBox.Text = text;
        //        textBox.CaretIndex = text.Length;
        //    }
        //    else
        //    {
        //        //入力のため-と-0と.を許容する
        //        if (text == "-" || text == "-0" || text.Last() == '.') return;
        //        double value;
        //        //数値なら値を検証してTextBoxに入れる
        //        if (double.TryParse(text, out value))
        //        {
        //            //値を検証し、矯正したならその値を入れる
        //            if (ValidateValue(ref value))
        //            {
        //                this.Value = value;
        //                //textBox.Text = value.ToString();
        //            }

        //            //var res = ValidateValue(value).ToString();
        //            //桁数を再現(ValidateValue(0.100)→0.1となるのを0.100に戻す)
        //            //var length = text.Length;
        //            //textBox.Text = res.PadRight(length, '0');
        //        }
        //    }
        //}

        /// <summary>
        /// 貼付けとカットを禁止する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste || e.Command == ApplicationCommands.Cut)
            {
                e.Handled = true;
            }
        }
        #endregion
        #endregion
    }
}
