# TurnDefense
ターン制の防衛ゲーム
# 用語集
* camp : 味方の陣地
* ally : 出陣所
* reserve : 控え
* frontline : 戦線
* StrategyManager.cursol : どこをカーソルが指定しているか。-1～-18がcamp -19：売却 -20：募集 -21：GO 1～(length+2)*(lanes)：戦線

# Character クラスについて
* キャラクタークラスのメンバである
CharaState
encampment
mass,lane
exists
reviveTurn
これらを使用してキャラの状態管理を行う。

## 募集直後・マージ直後
CharaState：Ally・Reserve
encampment：1～18
mass,lane：0
exists true
reviveTurn：0
## 出陣中
※陣地と戦線両方に同じキャラを指定する。
戦線のやつ
CharaState：Frontline
encampment：1～18
mass,lane：１～
exists：true
reviveTurn：0

陣地のやつ
CharaState：Waiting
encampment：1～18(戦線と同じものを指定)
mass,lane：１～(戦線と同じものを指定)
exists：true
reviveTurn：0
ステータス管理は戦線側で行う。
死んだときなどは戦線側をremoveし、陣地側だけを残す。
## 待機中
## 控えで待機中
## 売却、マージの素材
## 死んだ時
## 死んでいる時
## 復活時
## 持って移動時
## 進軍時
## マージ前
## ゴール時
