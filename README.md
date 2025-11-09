# HumanTowerBattle

Unityで作成した人間タワーバトルゲーム。

一部の実装は以下の記事を参考にしています：https://blog.livedoor.jp/asamasou/archives/5689856.html

## 概要
- Flaskサーバーから画像を受信してUnityで表示
- 人物を積み上げるゲーム

## 必要環境

### Unity側
- Unity 2021.3 以降

### Python側
- Python 3.x
- 以下のライブラリをインストールする必要があります：

| ライブラリ | 用途 |
|------------|------|
| Flask | Webサーバー構築 |
| OpenCV (`cv2`) | カメラ映像取得・画像処理 |
| MediaPipe | 人物のセグメンテーション |
| NumPy | マスク処理などの数値演算 |

### インストール方法

以下のコマンドで必要なライブラリをインストールできます：

```bash
pip install flask opencv-python mediapipe numpy
