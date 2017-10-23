# NumericUpDownControl
## 概要
このコントロールは、数値のみ入力できるTextBoxと、入力された数値をインクリメント・デクリメントできる2つのRepeatButtonからなります。
TextBoxへの貼付けとカットはできません。
値の変化量や上下限値、ループの設定も可能です。

## プロパティ
### Text
TextBoxへの入力を受ける為のプロパティです。
双方向バインドのために読取り専用にはしていませんが、基本的に読取り専用で使って下さい。

### Value
double型の実際の値です。
既定値は0で、初期値を設定することもできます。
この値が変わった時にValueChangedイベントを発生させます。

### Step
インクリメント・デクリメントした時のValueの変化量です。
既定値は1です。

### MaxValue
Valueの最大値です。これを設定するとValueがこの値を上回る時に強制的にこの値になります(IsLoopがfalseの時)。
既定値はnull(設定しない)です。

### MinValue
Valueの最小値です。これを設定するとValueがこの値を下回る時に強制的にこの値になります(IsLoopがfalseの時)。
既定値はnull(設定しない)です。

### IsLoop
値が上下限値を超えた時に値をループさせるかどうかの設定です。
上下限値のどちらかが設定されていなければ0になります。
既定値はfalseです。
**ループさせる時は上限値の設定に注意して下さい。**
<details>
ループさせる時はMaxValue + 1がMinValueになります。
例えば時計の分としてこのコントロールを使う場合、MaxValueは59,MinValueは0に設定します。
</details>

### IsReadOnly
TextBoxを読取り専用にするかどうかの設定です。
読取り専用だとValueの操作はRepeatButtonでしか行えなくなります。

### DecimalDigits
小数点以下の有効桁数です。
Valueはこの指定桁数まで四捨五入され、TextBoxには指定桁数以下の入力が制限されます。
既定値は0で、最大値は15までです。

### IsHoldDigits
Textの小数点以下の末尾の0を保持するかどうかの設定です。
既定値はtrueです。
<details>
(例)
Text = 0.100 の時
true : 0.100
false : 0.1
</details>

### UpContent
インクリメントボタンに表示するContentを設定します。

### DownContent
デクリメントボタンに表示するContentを設定します。

### DefaultTextColor
既定のTextBoxの文字色です。
既定値はColors.Blackです。

### NegativeValueTextColor
Valueが負の値の時のTextBoxの文字色です。
IsHighlightToNegativeValueがtrueの時のみ有効です。

### CurrentTextColor
現在のTextBoxの文字色です。
このプロパティは読取り専用です。

### IsHighlightToNegativeValue
Valueが負の値になる時にTextBoxの文字色を変更するかどうかの設定です。
既定値はfalseです。

### TextBoxBorderBrush
TextBoxの枠線のブラシです。
既定値はBrushes.LightGrayです。

### TextBoxBorderThickness
TextBoxの枠線の太さです。
既定値は1,1,1,1です。

### HorizontalValueAlignment
TextBoxにTextを表示する水平方向の位置です。
既定値はHorizontalAlignment.Centerです。

### TextBoxWidth
TextBoxの幅です。

### ButtonWidth
RepeatButtonの幅です。

## メソッド
### Increment()
ValueにStep値を加算してValueIncrementedイベントを発生させます。

### Increment(double step)
Valueに指定した値を加算してValueIncrementedイベントを発生させます。

### Decrement()
ValueからStep値を減算してValueDecrementedイベントを発生させます。

### Decrement(double step)
Valueから指定した値を減算してValueDecrementedイベントを発生させます。

## イベント
### ValueIncremented(DoubleChangedEventArgs e)
IncrementメソッドによりValueを加算した時に発生します。

### ValueDecremented(DoubleChangedEventArgs e)
DecrementメソッドによりValueを減算した時に発生します。

### ValueChanged(DoubleChangedEventArgs e)
Value値が変化した時に発生します。

### Looped(LoopedEventArgs e)
Valueがループした時に発生します。
