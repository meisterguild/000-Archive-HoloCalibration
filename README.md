# HoloCalibration
デモ等でHoloLensを装着する際に正しいポジション（描画領域がすべて見える位置で装着できているか）を確認するためのアプリケーション

## 使い方
1. 実際のアプリケーションのシーンを作成する。
2. 最初に表示する空のシーンを作成しHoloLens用のカメラ設定を実施
3. HoloCalibration\prefabs\ViewCalibration.prefabをHierarhcyに追加
4. 追加したオブジェクトのInspectorにてViewCalibrationのプロパティ設定を実施

## サンプルの使用方法
プロジェクトを開き、Open SceneからHoloCalibrationSamplesを開き実行する。

## ViewCalibrationのプロパティ
ViewCalibrationプロパティは以下の通り。

|プロパティ名|説明|
|:-:|:-:|
|Enabled Tutorial|装着確認後AirTapの練習を実施するか。チェックがある場合、ランダムに３Dオブジェクトが表示され正しくAirTapできると繰り返しランダムでオブジェクトが表示される。中央の常に表示されるオブジェクトをTapすることでシーンの切り替えが発生する。|
|MarkObjectSets|装着位置を確認するために対角線上に表示するオブジェクトを定義する。Sizeで中心～角の間に設置するオブジェクトの数を指定すると、生成する3Dオブジェクトを設定するプロパティが表示されるので、任意に作成したオブジェクトを設定する。|
|Target|チュートリアルにおいてランダムに表示されるターゲットとなる３Dオブジェクトを設定する。|
|ExitTarget|チュートリアルにおいて中央に表示されるオブジェクト。Tapすることでシーンの移動を行う。|
|SceneName|移動するシーンの名前を設定する。|
