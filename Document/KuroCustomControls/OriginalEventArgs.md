# OriginalEventArgs
## 概要
独自イベントに値を流す為のEventArgsを提供します。

## EventArgs
### DoubleChangedEventArgs
double型を持つイベントデータを提供します。
+ double NewValue
 変更後のValue
+ double OldValue
 変更前のValue

### IntChangedEventArgs
int型を持つイベントデータを提供します。
+ int NewValue
 変更後のValue
+ int OldValue
 変更前のValue

### TimeChangedEventArgs
Time変更時のイベントデータを提供します。
+ TimeSpan NewTime
 変更後のTimeSpan
+ TimeSpan OldTime
 変更前のTimeSpan
+ TimeSpan SubtractedTime
 変更後と変更前のTimeSpanの差(NewTim - OldTime)

### LoopedEventArgs
ループした時に、最大値を超えてループしたかどうかのイベントデータを提供します。
+ IsMaxOver
 最大値を超えてループしたか(falseなら最小値を下回ってのループ)
