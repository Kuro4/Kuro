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
        [Description("TextBoxに表示するテキストです。"), Category("共通"),ReadOnly(true),Browsable(false)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDownControl), new PropertyMetadata("0", OnTextChanged, OnCoerceText));
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
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDownControl), new PropertyMetadata((double)0,OnValueChanged, OnCoerceValue));
        #endregion
        #region DecimalDigitsプロパティ
        [Description("小数点以下の有効桁数です(15まで)。\r\nValueは指定桁まで四捨五入され、Textは指定桁以下に入力できなくなります。"), Category("共通")]
        public int DecimalDigits
        {
            get { return (int)GetValue(DecimalDigitsProperty); }
            set { SetValue(DecimalDigitsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DecimalDigits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecimalDigitsProperty =
            DependencyProperty.Register("DecimalDigits", typeof(int), typeof(NumericUpDownControl), new PropertyMetadata(0,null, OnCoerceDecimalDigits));
        #endregion
        #region IsHoldDigitsプロパティ
        [Description("trueならValue変更時、小数点以下の末尾の0を保持します。\r\n(例：[true]0.100→0.100 , [false]0.100→0.1)"), Category("共通")]
        public bool IsHoldDigits
        {
            get { return (bool)GetValue(IsHoldDigitsProperty); }
            set { SetValue(IsHoldDigitsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsHoldDigits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHoldDigitsProperty =
            DependencyProperty.Register("IsHoldDigits", typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(true, OnValueChanged));
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
            DependencyProperty.Register("UpContent", typeof(object), typeof(NumericUpDownControl), new PropertyMetadata("▲"));
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
            DependencyProperty.Register("DownContent", typeof(object), typeof(NumericUpDownControl), new PropertyMetadata("▼"));
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
            DependencyProperty.Register("MaxValue", typeof(double?), typeof(NumericUpDownControl), new PropertyMetadata(null, null, OnCoerceMaxValue));
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
            DependencyProperty.Register("MinValue", typeof(double?), typeof(NumericUpDownControl), new PropertyMetadata(null, null, OnCoerceMinValue));
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
            DependencyProperty.Register("IsHighlightToNegativeValue", typeof(bool), typeof(NumericUpDownControl), new PropertyMetadata(false, OnValueChanged));
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
        private static readonly DependencyPropertyKey CurrentTextColorPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "CurrentTextColor",
                typeof(SolidColorBrush),
                typeof(NumericUpDownControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static readonly DependencyProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.DependencyProperty;
        public SolidColorBrush CurrentTextColor
        {
            get { return (SolidColorBrush)GetValue(CurrentTextColorProperty); }
            private set { this.SetValue(CurrentTextColorPropertyKey, value); }
        }
        #endregion
        #region TextBoxBorderBrushプロパティ
        [Description("TextBoxの枠の色です。"), Category("ブラシ")]
        public Brush TextBoxBorderBrush
        {
            get { return (Brush)GetValue(TextBoxBorderBrushProperty); }
            set { SetValue(TextBoxBorderBrushProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextBoxBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxBorderBrushProperty =
            DependencyProperty.Register("TextBoxBorderBrush", typeof(Brush), typeof(NumericUpDownControl), new PropertyMetadata(Brushes.LightGray));
        #endregion
        #region TextBoxBorderThicknessプロパティ
        [Description("TextBoxの枠線の太さです。"), Category("外観")]
        public Thickness TextBoxBorderThickness
        {
            get { return (Thickness)GetValue(TextBoxBorderThicknessProperty); }
            set { SetValue(TextBoxBorderThicknessProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextBoxBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxBorderThicknessProperty =
            DependencyProperty.Register("TextBoxBorderThickness", typeof(Thickness), typeof(NumericUpDownControl), new PropertyMetadata(new Thickness(1,1,1,1)));
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
        /// ValueをStepの値だけ加算する
        /// </summary>
        public void Increment() { this.Value += this.Step; }
        /// <summary>
        /// Valueを引数のstepの値だけ加算する
        /// </summary>
        /// <param name="step"></param>
        public void Increment(double step) { this.Value += step; }
        /// <summary>
        /// ValueをStepの値だけ減算する
        /// </summary>
        public void Decrement() { this.Value -= this.Step; }
        /// <summary>
        /// Valueを引数のstepの値だけ減算する
        /// </summary>
        /// <param name="step"></param>
        public void Decrement(double step) { this.Value -= step; }
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
        #region Valueの検証・矯正
        /// <summary>
        /// 引数の値が上下限値を超えているか検証し、超えていれば矯正した値を返す
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>矯正値</returns>
        private double ValidateValue(double value)
        {
            var resMax = value;
            var resMin = value;
            if (ValidateMaxValue(ref resMax)) return resMax;
            else if (ValidateMinValue(ref resMin)) return resMin;
            else return value;
        }
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
        /// Text変更時の強制コールバックメソッド
        /// 小数点以下の桁数を指定桁数で制限する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceText(DependencyObject d,object baseValue)
        {
            var self = (NumericUpDownControl)d;
            var newValue = (string)baseValue;
            if (newValue.Contains("."))
            {
                //文字数
                var length = newValue.Length;
                //少数桁数 = 文字数 - "."の位置(indexが0からのカウントのため-1)
                var digit = length - newValue.IndexOf(".") - 1;
                if(digit > self.DecimalDigits) return newValue.Substring(0, length - (digit - self.DecimalDigits));
            }
            return baseValue;
        }
        /// <summary>
        /// Text変更時のコールバックメソッド
        /// Textがdouble型にパースできるならValue伝播させる
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTextChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            double value;
            if (double.TryParse(e.NewValue.ToString(), out value)) self.Value = value;
        }
        /// <summary>
        /// Value変更時の強制コールバックメソッド
        /// 値を有効範囲内に強制して流す
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceValue(DependencyObject d, object baseValue)
        {
            var self = (NumericUpDownControl)d;
            var value = (double)baseValue;
            //値の検証、強制
            value = self.ValidateValue(value);
            //指定桁数で丸める
            return Math.Round(value, self.DecimalDigits);
        }
        /// <summary>
        /// Value変更時のコールバックメソッド
        /// 値をTextへ伝搬させ、文字色を更新する
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (NumericUpDownControl)d;
            self.Text = self.IsHoldDigits ? self.HoldDigits(self.Value, self.Text) : self.Value.ToString();
            self.UpdateValueColor();
        }
        /// <summary>
        /// MinValue&lt;MaxValueの関係が正しいかどうかを判定し、矯正値を返す
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceMaxValue(DependencyObject d, object baseValue)
        {
            var self = (NumericUpDownControl)d;
            var maxValue = (double?)baseValue;
            if (self.MinValue.HasValue && maxValue.HasValue)
                if (self.MinValue.Value > maxValue) return self.MinValue;
            return baseValue;
        }
        /// <summary>
        /// MinValue&lt;MaxValueの関係が正しいかどうかを判定し、矯正値を返す
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceMinValue(DependencyObject d, object baseValue)
        {
            var self = (NumericUpDownControl)d;
            var minValue = (double?)baseValue;
            if (self.MaxValue.HasValue && minValue.HasValue)
                if (self.MaxValue.Value < minValue) return self.MaxValue;
            return baseValue;
        }
        /// <summary>
        /// DecimalDigits変更時のコールバックメソッド
        /// 正の値に矯正する(最大値は15)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private static object OnCoerceDecimalDigits(DependencyObject d,object baseValue)
        {
            var newValue = (int)baseValue;
            newValue = Math.Abs(newValue);
            return newValue <= 15 ? newValue : 15;
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
        /// また、DecimalDigitsの制限値以下の少数桁の入力を禁止する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)e.Source;
            var text = textBox.Text;
            //小数点は1つだけ許容する(SelectedText内にあり、上書きする時も許容)
            if (e.Text == ".") e.Handled = text.Contains(".") && !textBox.SelectedText.Contains(".") || this.DecimalDigits == 0;
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
