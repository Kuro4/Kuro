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


        #region Hoursプロパティ
        [Description("時間を表します。")]
        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Hours.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimePickerControl), new PropertyMetadata(0));
        #endregion
        #region Minutesプロパティ
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Minutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimePickerControl), new PropertyMetadata(0));
        #endregion
        #region MinuteStepプロパティ
        [Description("分を増減させる時のの変化量です"),Category("共通")]
        public int MinutesStep
        {
            get { return (int)GetValue(MinutesStepProperty); }
            set { SetValue(MinutesStepProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MinuteStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesStepProperty =
            DependencyProperty.Register("MinutesStep", typeof(int), typeof(TimePickerControl), new PropertyMetadata(1));
        #endregion

        private double totalHours;
        public double TotalHours
        {
            get { return Time.TotalHours; }
            private set { totalHours = value; }
        }

        private double totalMinutes;
        public double TotalMinutes
        {
            get { return Time.TotalMinutes; }
            private set { totalMinutes = value; }
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
            private set { time = value; }
        }

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
            if (this.hour != null)
            {
                hour.ValueIncremented -= Hour_ValueIncremented;
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
                hour.ValueIncremented += Hour_ValueIncremented;
            }
            if (this.minute != null)
            {

            }
        }

        private void Hour_ValueIncremented(object sender, EventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        #endregion
    }
}
