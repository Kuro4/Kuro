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
        #region Timeプロパティ
        [Description("現在のHourとMinuteで構成されたTimeSpanです。(読取専用)"), Category("共通")]
        private static readonly DependencyPropertyKey TimePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "Time",
                typeof(TimeSpan),
                typeof(TimePickerControl),
                new PropertyMetadata(new TimeSpan(), OnTimeChanged));
        public static readonly DependencyProperty TimeProperty = TimePropertyKey.DependencyProperty;
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            private set { this.SetValue(TimePropertyKey, value); }
        }
        #endregion
        #region IsReadOnlyプロパティ
        [Description("TextBoxに入力できるようにするかどうかを示します。"), Category("共通")]
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TimePickerControl), new PropertyMetadata(false));
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
            System.Diagnostics.Debug.WriteLine("HourChanged!");
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
            System.Diagnostics.Debug.WriteLine("MinutesChanged!");
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
            System.Diagnostics.Debug.WriteLine("TimeChanged!(コールバック)");
            self.OnTimeChanged(self.DPCEToTCEA(e));
        }

        #region フィールド
        private NumericUpDownControl hour;
        private NumericUpDownControl minute;
        #endregion

        #region メソッド

        /// <summary>
        /// 時間と分を指定してTimeSpanインスタンスを返す
        /// </summary>
        /// <param name="hours">時間</param>
        /// <param name="minutes">分</param>
        /// <returns></returns>
        private TimeSpan CreateTime(int hours,int minutes) {return new TimeSpan(hours, minutes, 0); }
        /// <summary>
        /// DependencyPropertyChangedEventArgsをTimeChangedEventArgsに変換します。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private TimeChangedEventArgs DPCEToTCEA(DependencyPropertyChangedEventArgs e)
        {
            return new TimeChangedEventArgs((TimeSpan)e.NewValue, (TimeSpan)e.OldValue);
        }

        /// <summary>
        /// イベントの登録・解除
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // 前のテンプレートのコントロールの後処理(イベントハンドラの解除)
            if (this.hour != null)
            {
            }
            if (this.minute != null)
            {
            }

            // テンプレートからコントロールの取得
            this.hour = this.GetTemplateChild("PART_Hour") as NumericUpDownControl;
            this.minute = this.GetTemplateChild("PART_Minute") as NumericUpDownControl;
            // イベントハンドラの登録
            if (this.hour != null)
            {
            }
            if (this.minute != null)
            {
            }
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
        protected virtual void OnMinutesChanged(IntChangedEventArgs e)
        {
            MinutesChanged?.Invoke(this, e);
        }
        public event TimeChangedEventHandler TimeChanged;
        protected virtual void OnTimeChanged(TimeChangedEventArgs e)
        {
            TimeChanged?.Invoke(this, e);
        }
        #endregion
    }
}
