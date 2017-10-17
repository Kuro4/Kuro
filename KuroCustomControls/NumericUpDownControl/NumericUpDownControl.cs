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
    /// <summary>
    /// 数値を指定した値だけインクリメント、デクリメントできるスピンボタンを持つTextBox
    /// </summary>
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
            DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDownControl), new PropertyMetadata("0", TextChanged));
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
        #region DecimalDigitsプロパティ
        [Description("小数点以下の有効桁数です。指定桁以下の値は切り捨てされます。(表示状は0が増えます)"), Category("共通")]
        public int DecimalDigits
        {
            get { return (int)GetValue(DecimalDigitsProperty); }
            set { SetValue(DecimalDigitsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DecimalDigits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecimalDigitsProperty =
            DependencyProperty.Register("DecimalDigits", typeof(int), typeof(NumericUpDownControl), new PropertyMetadata(0));
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
        #region DefaultTextColorプロパティ
        [Description("既定のTextの文字色です。"), Category("共通")]
        public Color DefaultTextColor
        {
            get { return (Color)GetValue(DefaultTextColorProperty); }
            set { SetValue(DefaultTextColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DefaultTextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultTextColorProperty =
            DependencyProperty.Register("DefaultTextColor", typeof(Color), typeof(NumericUpDownControl), new PropertyMetadata(Colors.Black, ColorChanged));
        #endregion
        #region NegativeValueTextColorプロパティ
        [Description("Valueが負の値になった時の文字色です。IsHighlightToNegativeValueがtrueの時のみ有効です。"), Category("共通")]
        public Color NegativeValueTextColor
        {
            get { return (Color)GetValue(NegativeValueTextColorProperty); }
            set { SetValue(NegativeValueTextColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for NegativeValueTextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NegativeValueTextColorProperty =
            DependencyProperty.Register("NegativeValueTextColor", typeof(Color), typeof(NumericUpDownControl), new PropertyMetadata(Colors.Red, ColorChanged));
        #endregion
        #region CurrentTextColorプロパティ
        [Description("現在のTextの色です。(読取専用)"), Category("共通")]
        private static readonly DependencyPropertyKey CurrentTexteColorPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "CurrentTextColor",
                typeof(SolidColorBrush),
                typeof(NumericUpDownControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static readonly DependencyProperty CurrentTextColorProperty = CurrentTexteColorPropertyKey.DependencyProperty;
        public SolidColorBrush CurrentTextColor
        {
            get { return (SolidColorBrush)GetValue(CurrentTextColorProperty); }
            private set { this.SetValue(CurrentTexteColorPropertyKey, value); }
        }
        #endregion
        #endregion
        #region フィールド
        // XAMLで名前を付けた(イベントを使用する)コントロールの格納用変数
        private TextBox valueBox;
        private RepeatButton upButton;
        private RepeatButton downButton;
        #endregion
        #region メソッド
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
            if (this.valueBox != null)
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
            if (this.valueBox != null)
            {
                this.valueBox.PreviewTextInput += ValueBox_PreviewTextInput;
                CommandManager.AddPreviewExecutedHandler(this.valueBox, ValueBox_PreviewExecuted);
            }
        }
        /// <summary>
        /// 現在のValueの色を更新する
        /// </summary>
        private void UpdateValueColor()
        {
            Color res;
            if (this.IsHighlightToNegativeValue)
            {
                if (this.Value >= 0) res = this.DefaultTextColor;
                else res = this.NegativeValueTextColor;
            }
            else res = this.DefaultTextColor;
            if (this.CurrentTextColor.Color != res) this.CurrentTextColor = new SolidColorBrush(res);
        }
        /// <summary>
        /// 小数点以下の桁数をtextに合わせて整形して返す(例：value(10),text("10.000")→return("10.000"))
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private string HoldDigits(double value,string text)
        {
            var valueText = value.ToString();
            //Textが小数点を含んでいない場合、そのままの値を返す
            if (!text.Contains(".")) return valueText;
            //valueの小数点以下の桁数
            var valueDigit = valueText.Length - (valueText.IndexOf(".") + 1);
            //textの小数点以下の桁数
            var textDigit = text.Length - (text.IndexOf(".") + 1);
            //Valueが小数点以下の値を持つ場合(例：10.100)
            if (Math.Abs(value) % 1 > 0) return valueText.PadRight(valueText.Length + (textDigit - valueDigit), '0');
            //Valueが小数点以下の値を持たない場合(例：10.000)
            else return (valueText + ".").PadRight((valueText.Length + 1) + textDigit, '0');
        }
        /// <summary>
        /// ValueをStepの値だけ加算する
        /// </summary>
        public void Increment() { this.Value += Step; }
        /// <summary>
        /// ValueをStepの値だけ減算する
        /// </summary>
        public void Decrement() { this.Value -= Step; }
        #region Valueの検証・矯正
        /// <summary>
        /// 引数の値が上下限値を超えているか検証し、超えていれば矯正した値を参照引数に入れ、trueを返す
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>矯正したかどうか</returns>
        private bool ValidateValue(ref double value)
        {
            var resMax = value;
            var resMin = value;
            if (ValidateMaxValue(ref resMax)) value = resMax;
            else if (ValidateMinValue(ref resMin)) value = resMin;
            else return false;
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
        /// Text変更時のコールバックメソッド
        /// Textがdouble型にパースできるならValueへ入れる
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void TextChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            double value;
            if (double.TryParse(self.Text, out value)) self.Value = value;
        }
        /// <summary>
        /// Value変更時のコールバックメソッド
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            //プロパティはrefで渡せないのでローカル変数に入れる
            var value = self.Value;
            var res = self.ValidateValue(ref value);
            //値を丸める
            var roundValue = Math.Floor(value * Math.Pow(10, self.DecimalDigits)) / Math.Pow(10, self.DecimalDigits);
            self.Value = roundValue;
            //桁数を保持させたValueをTextに入れる
            self.Text = self.HoldDigits(self.Value, self.Text);
            self.UpdateValueColor();
        }
        /// <summary>
        /// Color変更時のコールバックメソッド
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// UpボタンクリックでValueをStepの値だけ加算する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpClick(object sender, RoutedEventArgs e) { this.Increment(); }
        /// <summary>
        /// DownボタンクリックでValueをStepの値だけ減算する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownClick(object sender, RoutedEventArgs e) { this.Decrement(); }
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
            else if (text.Contains("-") && textBox.CaretIndex == 0 && textBox.SelectedText != text) e.Handled = true;
            //数字のみ許容する
            else e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
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
