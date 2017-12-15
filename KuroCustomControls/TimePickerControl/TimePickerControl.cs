using KuroUtilities.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KuroCustomControls
{
    public class TimePickerControl : Control
    {
        static TimePickerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerControl),
                new FrameworkPropertyMetadata(typeof(TimePickerControl)));
        }
        #region プロパティ
        #region Hoursプロパティ
        [Description("時間を表します。"),Category("共通")]
        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Hours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimePickerControl), new PropertyMetadata(0, OnHoursChanged));
        #endregion
        #region Minutesプロパティ
        [Description("分を表します。"), Category("共通")]
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Minutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimePickerControl), new PropertyMetadata(0, OnMinutesChanged));
        #endregion
        #region Timeプロパティ
        private static readonly DependencyPropertyKey TimePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "Time",
                typeof(TimeSpan),
                typeof(TimePickerControl),
                new PropertyMetadata(new TimeSpan(), OnTimeChanged));
        public static readonly DependencyProperty TimeProperty = TimePropertyKey.DependencyProperty;
        [Description("現在のHourとMinuteで構成されたTimeSpanです。(読取専用)"), Category("共通")]
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            private set { this.SetValue(TimePropertyKey, value); }
        }
        #endregion
        #region HoursStepプロパティ
        [Description("時間を増減させる時の変化量です。"), Category("共通")]
        public int HoursStep
        {
            get { return (int)GetValue(HoursStepProperty); }
            set { SetValue(HoursStepProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinuteStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursStepProperty =
            DependencyProperty.Register("HoursStep", typeof(int), typeof(TimePickerControl), new PropertyMetadata(1));
        #endregion
        #region MinuteStepプロパティ
        [Description("分を増減させる時の変化量です。"),Category("共通")]
        public int MinutesStep
        {
            get { return (int)GetValue(MinutesStepProperty); }
            set { SetValue(MinutesStepProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinuteStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesStepProperty =
            DependencyProperty.Register("MinutesStep", typeof(int), typeof(TimePickerControl), new PropertyMetadata(1));
        #endregion
        #region IsReadOnlyHoursプロパティ
        [Description("HoursのTextBoxを読取り専用にするかどうかを示します。"), Category("共通")]
        public bool IsReadOnlyHours
        {
            get { return (bool)GetValue(IsReadOnlyHoursProperty); }
            set { SetValue(IsReadOnlyHoursProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsReadOnlyHours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyHoursProperty =
            DependencyProperty.Register("IsReadOnlyHours", typeof(bool), typeof(TimePickerControl), new PropertyMetadata(false));
        #endregion
        #region IsReadOnlyMinutesプロパティ
        [Description("MinutesのTextBoxを読取り専用にするかどうかを示します。"), Category("共通")]
        public bool IsReadOnlyMinutes
        {
            get { return (bool)GetValue(IsReadOnlyMinutesProperty); }
            set { SetValue(IsReadOnlyMinutesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsReadOnlyMinutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyMinutesProperty =
            DependencyProperty.Register("IsReadOnlyMinutes", typeof(bool), typeof(TimePickerControl), new PropertyMetadata(false));
        #endregion
        #region DelimiterContentプロパティ
        [Description("時間と分の区切りに使うContentです。"), Category("共通")]
        public object DelimiterContent
        {
            get { return (object)GetValue(DelimiterContentProperty); }
            set { SetValue(DelimiterContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DelimiterContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelimiterContentProperty =
            DependencyProperty.Register("DelimiterContent", typeof(object), typeof(TimePickerControl), new PropertyMetadata("："));


        #endregion
        #endregion
        #region フィールド
        private NumericUpDownControl hour;
        private NumericUpDownControl minute;
        #endregion
        #region メソッド
        /// <summary>
        /// イベントの登録・解除
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // 前のテンプレートのコントロールの後処理(イベントハンドラの解除)
            if (this.minute != null)
            {
                minute.Looped -= Minute_Looped;
            }
            // テンプレートからコントロールの取得
            this.hour = this.GetTemplateChild("PART_Hour") as NumericUpDownControl;
            this.minute = this.GetTemplateChild("PART_Minute") as NumericUpDownControl;
            // イベントハンドラの登録
            if (this.minute != null)
            {
                minute.Looped += Minute_Looped;
            }
        }
        /// <summary>
        /// HoursをHoursStepの値だけ加算する
        /// </summary>
        public void IncrementHours()
        {
            this.hour.Increment();
        }
        /// <summary>
        /// Hoursを指定した値だけ加算する
        /// </summary>
        public void IncrementHours(int step)
        {
            this.hour.Increment(step);
        }
        /// <summary>
        /// HoursをHoursStepの値だけ減算する
        /// </summary>
        public void DecrementHours()
        {
            this.hour.Decrement();
        }
        /// <summary>
        /// Hoursを指定した値だけ減算する
        /// </summary>
        public void DecrementHours(int step)
        {
            this.hour.Decrement(step);
        }
        /// <summary>
        /// MinutesをMinutesStepの値だけ加算する
        /// </summary>
        public void IncrementMinutes()
        {
            this.minute.Increment();
        }
        /// <summary>
        /// Minutesを指定した値だけ加算する
        /// </summary>
        public void IncrementMinutes(int step)
        {
            this.minute.Increment(step);
        }
        /// <summary>
        /// MinutesをMinutesStepの値だけ減算する
        /// </summary>
        public void DecrementMinutes()
        {
            this.minute.Decrement();
        }
        /// <summary>
        /// MinutesをMinutesStepの値だけ減算する
        /// </summary>
        public void DecrementMinutes(int step)
        {
            this.minute.Decrement(step);
        }
        /// <summary>
        /// 時間と分を指定してTimeSpanインスタンスを返す
        /// </summary>
        /// <param name="hours">時間</param>
        /// <param name="minutes">分</param>
        /// <returns></returns>
        private TimeSpan CreateTime(int hours, int minutes) { return new TimeSpan(hours, minutes, 0); }
        #region コールバック
        /// <summary>
        /// Hours変更時のコールバックメソッド
        /// OnHourChangedイベントを発火させる
        /// Timeにも伝搬させる
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnHoursChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (TimePickerControl)d;
            self.OnHourChanged(new IntChangedEventArgs((int)e.NewValue,(int)e.OldValue));
            self.Time = self.CreateTime((int)e.NewValue, self.Minutes);
        }
        /// <summary>
        /// Minutes変更時のコールバックメソッド
        /// OnMinutesChangedイベントを発火させる
        /// Timeにも伝搬させる
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMinutesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (TimePickerControl)d;
            self.OnMinutesChanged(new IntChangedEventArgs((int)e.NewValue, (int)e.OldValue));
            self.Time = self.CreateTime(self.Hours,(int)e.NewValue);
        }
        /// <summary>
        /// Time変更時のコールバックイベント
        /// OnTimeChangedイベントを発火させる
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTimeChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var self = (TimePickerControl)d;
            self.OnTimeChanged(new TimeChangedEventArgs((TimeSpan)e.NewValue, (TimeSpan)e.OldValue));
        }
        #endregion
        #region イベント処理
        /// <summary>
        /// 分がループした時に時をインクリメントする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minute_Looped(object sender, LoopedEventArgs e)
        {
            if (e.IsMaxOver) this.hour.Increment();
            else this.hour.Decrement();
        }
        #endregion
        #region イベント
        public event IntChangedEventHandler HoursChanged;
        /// <summary>
        /// 時間変更時に発生するイベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnHourChanged(IntChangedEventArgs e)
        {
            HoursChanged?.Invoke(this, e);
        }
        public event IntChangedEventHandler MinutesChanged;
        /// <summary>
        /// 分変更時に発生するイベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMinutesChanged(IntChangedEventArgs e)
        {
            MinutesChanged?.Invoke(this, e);
        }
        public event TimeChangedEventHandler TimeChanged;
        /// <summary>
        /// 時間(TimeSpan)変更時に発生するイベント
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTimeChanged(TimeChangedEventArgs e)
        {
            TimeChanged?.Invoke(this, e);
        }
        #endregion
        #endregion
    }
}
