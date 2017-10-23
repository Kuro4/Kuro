# TimePickerControl
## 概要
このコントロールは2つのNumericUpDownControlと1つのLabelからなります。
このコントロールは時間と分を選択するのに便利ですが、日や秒はなどサポートしていません。
Timeプロパティから選択した時間を取得できます。

## プロパティ
### Hours
int型の時間です。
変更時にはHoursChangedイベントを発生させます。
既定値は0です。

### Minutes
int型の分です。
変更時にはMinutesChangedイベントを発生させます。
既定値は0です。

### Time
現在のHoursとMinutesによって生成されるTimeSpan型の時間です。
このプロパティは読取り専用です。
変更時にはTimeChangedイベントを発生させます。

### HoursStep
NumericUpDownControlのインクリメント・デクリメントでの値の変化量です。
既定値は1です。

### MinutesStep
NumericUpDownControlのインクリメント・デクリメントでの値の変化量です。
既定値は1です。

### IsReadOnlyHours
TextBoxを読取り専用にするかどうかの設定です。
既定値はfalseです。

### IsReadOnlyMinutes
TextBoxを読取り専用にするかどうかの設定です。
既定値はfalseです。

### DelimiterContent
時間と分の区切りに使うContentです。
既定値は：です。

## メソッド
### IncrementHours()
HoursにHoursStepの値を加算します。

### IncrementHours(int step)
Hoursに指定した値を加算します。

### DecrementHours()
HoursからHoursStepの値を減算します。

### DecrementHours(int step)
Hoursから指定した値を減算します。

### IncrementMinutes()
MinutesにMinutesStepの値を加算します。

### IncrementMinutes(int step)
Minutesに指定した値を加算します。

### DecrementMinutes()
MinutesからMinutesStepの値を減算します。

### DecrementMinutes(int step)
Minutesから指定した値を減算します。

## イベント
### HourChanged(IntChangedEventArgs e)
時間が変更した時に発生します。

### MinutesChanged(IntChangedEventArgs e)
分が変更した時に発生します。

### TimeChanged(TimeChangedEventArgs e)
時間(TimeSpan)が変更した時に発生します。
