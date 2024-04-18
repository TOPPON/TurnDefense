using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyManager : MonoBehaviour
{
    public static StrategyManager Instance;
    // Start is called before the first frame update
    public enum StrategyState
    {
        Normal,
        Having,
        Merging,
        Recruiting
    }
    public StrategyState strategyState;
    int cursol = 1; // -1から-18 キャンプ -19 売却 -20 募集 -21 GO 1からLanes*LaneLength 戦線
    int normalCursol = 1;//通常状態のカーソル
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        strategyState = StrategyState.Normal;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FinishMerge()
    {
        strategyState = StrategyState.Normal;
    }
    public int CalcSellPrice(Character target)
    {
        print("CalcSellPrice:" + target.rarity * 5);
        return target.rarity * 5;
    }
    //決定ボタンを押したときのアクション
    public void PushAButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
                //通常状態
                if (cursol == -21)//GO
                {
                    GameManager.Instance.PushGOButton();
                }
                else if (cursol == -20)//募集
                {
                    if (GameManager.Instance.money >= 5 &&
                        BattleStageManager.Instance.CheckEnableAddCharacter())
                    {
                        GameManager.Instance.ReduceMoney(5);
                        strategyState = StrategyState.Recruiting;
                    }
                }
                else if (cursol == -19)//売却
                {
                    //何もしない
                }
                else if (cursol < 0)
                {
                    int campIndex = -cursol - 1;
                    if (BattleStageManager.Instance.camp[campIndex].exists)
                    {
                        if (BattleStageManager.Instance.camp[campIndex].charaState == Character.CharaState.Ally || BattleStageManager.Instance.camp[campIndex].charaState == Character.CharaState.Reserve)
                        {
                            strategyState = StrategyState.Having;
                            normalCursol = cursol;
                            BattleStageDisplayManager.Instance.ActivateNormalCursol(cursol);
                            BattleInformationDisplayManager.Instance.RefreshSellPriceText(CalcSellPrice(BattleStageManager.Instance.camp[campIndex]));
                        }
                    }
                }
                else if (cursol > 0)
                {
                    Vector2 frontlinePos = BattleStageManager.Instance.GetFrontlineLaneAndMassByCursol(cursol);
                    int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByCursol(cursol);
                    if (BattleStageManager.Instance.frontline[frontlineIndex].exists)
                    {
                        if (frontlinePos.y == 1)
                        {
                            strategyState = StrategyState.Having;
                            normalCursol = cursol;
                            BattleStageDisplayManager.Instance.ActivateNormalCursol(cursol);
                            BattleInformationDisplayManager.Instance.RefreshSellPriceText(CalcSellPrice(BattleStageManager.Instance.frontline[frontlineIndex]));
                        }
                    }
                }
                else
                {
                    print("error! cursol is invalid");
                }
                //カーソル先にプレイヤーがいれば状態変化、Havingに以降してカーソルを変える
                //募集マスなら募集する
                //GOマスならターンを進める
                break;
            case StrategyState.Having:
                if (cursol == -21)//GO
                {
                    //何もしない
                }
                else if (cursol == -20)//募集
                {
                    //何もしない
                }
                else if (cursol == -19)//売却
                {
                    //売却
                    if (normalCursol < 0)
                    {
                        int campIndex = -normalCursol - 1;
                        int income = CalcSellPrice(BattleStageManager.Instance.camp[campIndex]);
                        GameManager.Instance.AddMoney(income);
                        BattleStageManager.Instance.RemoveCharacter(campIndex, -1);
                        strategyState = StrategyState.Normal;
                        cursol = normalCursol;
                        BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                        BattleStageDisplayManager.Instance.DeactivateNormalCursol();
                    }
                    else if (normalCursol > 0)
                    {
                        int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByCursol(cursol);
                        int income = CalcSellPrice(BattleStageManager.Instance.frontline[frontlineIndex]);
                        GameManager.Instance.AddMoney(income);
                        BattleStageManager.Instance.RemoveCharacter(-1, frontlineIndex);
                        strategyState = StrategyState.Normal;
                        cursol = normalCursol;
                        BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                        BattleStageDisplayManager.Instance.DeactivateNormalCursol();
                    }
                }
                else if (cursol < 0)
                {
                    int campIndex = -cursol - 1;
                    //空きます奈良移動させる
                    //誰かいたら星が一緒の場合はマージ画面へ
                    //それ以外は何もしない
                }
                else if (cursol > 0)
                {
                    //空きますなら移動させる
                    //誰かいたら何もしない
                }
                else
                {
                    print("error! cursol is invalid");
                }
                //キャラクターを所持した状態
                //同じマスなら置く(ステートだけを変更する)
                //マージ可能であればマージ画面へ
                //空きマスなら移動させる
                //売却マスなら売る
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushAButton();
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }
    }
    public void PushBButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
                //通常状態
                strategyState = StrategyState.Merging;
                Character target1 = new Character();
                target1.maxHp = Random.Range(1, 8);
                target1.nowHp = target1.maxHp; // 現在の体力
                target1.power = Random.Range(1, 8); // 攻撃力
                target1.attackSpd = Random.Range(1, 8) + 4; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
                target1.skillType = Random.Range(1, 3); // スキルの種類
                target1.skillPoint = Random.Range(1, 8); // スキルポイント、０～１６で表現
                target1.skillLevel = target1.skillPoint / 3; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
                target1.rarity = Random.Range(1, 5);
                Character target2 = new Character();
                target2.maxHp = Random.Range(1, 8);
                target2.nowHp = target2.maxHp; // 現在の体力
                target2.power = Random.Range(1, 8); // 攻撃力
                target2.attackSpd = Random.Range(1, 8) + 4; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
                target2.skillType = Random.Range(1, 3); // スキルの種類
                target2.skillPoint = Random.Range(1, 8); // スキルポイント、０～１６で表現
                target2.skillLevel = target2.skillPoint / 3; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
                target2.rarity = Random.Range(1, 5);
                MergeManager.Instance.Activate(target1, target2);
                break;
            case StrategyState.Having:
                strategyState = StrategyState.Normal;
                cursol = normalCursol;
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                BattleStageDisplayManager.Instance.DeactivateNormalCursol();
                //キャラとカーソルを戻す
                //キャラクターを所持した状態
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushBButton();
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }
    }

    //左ボタンを押したときのアクション
    public void PushLeftButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //通常状態
            case StrategyState.Having:
                //キャラクターを所持した状態
                if (cursol < 0) //自陣
                {
                    if (cursol >= -18)//キャンプ
                    {
                        if (cursol == -1 | cursol == -7 | cursol == -13)
                        {
                            //キャンプ左端は売却へ
                            cursol = -19;
                        }
                        else
                        {
                            cursol += 1;
                        }
                    }
                    else if (cursol == -19) //売却
                    {
                        //動かない
                    }
                    else if (cursol == -20) //募集
                    {
                        //キャンプの右下に移動する
                        cursol = -18;
                    }
                    else if (cursol == -21) //GO
                    {
                        //募集に移動する
                        cursol = -20;
                    }
                }
                else if (cursol > 0) //戦線
                {
                    if (cursol <= BattleStageManager.Instance.laneLength + 2)
                    {
                        //一番左の列は動かない
                    }
                    else
                    {
                        //一列左に移動
                        cursol -= BattleStageManager.Instance.laneLength + 2;
                    }
                }
                else
                {
                    print("error! invalid cursol");
                }
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushLeftButton();
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }
    }
    public void PushRightButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //通常状態
            case StrategyState.Having:
                //キャラクターを所持した状態
                if (cursol < 0) //自陣
                {
                    if (cursol >= -18)//キャンプ
                    {
                        if (cursol == -6 | cursol == -12 | cursol == -18)
                        {
                            //キャンプ右端は募集へ
                            cursol = -20;
                        }
                        else
                        {
                            cursol -= 1;
                        }
                    }
                    else if (cursol == -19) //売却
                    {
                        //キャンプの左下に移動する
                        cursol = -13;
                    }
                    else if (cursol == -20) //募集
                    {
                        //GOに移動する
                        cursol = -21;
                    }
                    else if (cursol == -21) //GO
                    {
                        //移動しない
                    }
                }
                else if (cursol > 0) //戦線
                {
                    if (cursol > (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1))
                    {
                        //一番右の列は動かない
                    }
                    else
                    {
                        //一列右に移動
                        cursol += BattleStageManager.Instance.laneLength + 2;
                    }
                }
                else
                {
                    print("error! invalid cursol");
                }
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushRightButton();
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }
    }
    public void PushUpButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //通常状態
            case StrategyState.Having:
                //キャラクターを所持した状態
                if (cursol < 0) //自陣
                {
                    if (cursol >= -18)//キャンプ
                    {
                        if (cursol >= -6)
                        {
                            //キャンプ上端は戦線へ
                            cursol = (int)(((cursol * -1.0f) - 1) / (6 - 1) * (BattleStageManager.Instance.laneCount - 1) + 0.5f) * (BattleStageManager.Instance.laneLength + 2) + 1;
                        }
                        else
                        {
                            //キャンプを一行上に移動
                            cursol += 6;
                        }
                    }
                    else if (cursol == -19) //売却
                    {
                        //戦線の左下に移動
                        cursol = 1;
                    }
                    else if (cursol == -20) //募集
                    {
                        //戦線の右下に移動
                        cursol = (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1) + 1;
                    }
                    else if (cursol == -21) //GO
                    {
                        //戦線の右下に移動
                        cursol = (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1) + 1;
                    }
                }
                else if (cursol > 0) //戦線
                {
                    if (cursol % (BattleStageManager.Instance.laneLength + 2) == 0)
                    {
                        //一番上の列は動かない
                    }
                    else
                    {
                        //一行上に移動
                        cursol += 1;
                    }
                }
                else
                {
                    print("error! invalid cursol");
                }
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                break;
            case StrategyState.Merging:
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }

    }
    public void PushDownButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //通常状態
            case StrategyState.Having:
                //キャラクターを所持した状態
                if (cursol < 0) //自陣
                {
                    if (cursol >= -18)//キャンプ
                    {
                        if (cursol <= -13)
                        {
                            //キャンプ下端は動かない
                        }
                        else
                        {
                            //キャンプを一行下に移動
                            cursol -= 6;
                        }
                    }
                    else if (cursol == -19) //売却
                    {
                        //動かない
                    }
                    else if (cursol == -20) //募集
                    {
                        //動かない
                    }
                    else if (cursol == -21) //GO
                    {
                        //動かない
                    }
                }
                else if (cursol > 0) //戦線
                {
                    if (cursol % (BattleStageManager.Instance.laneLength + 2) == 1)
                    {
                        //キャンプに移動
                        cursol = (int)((int)(cursol / (BattleStageManager.Instance.laneLength + 2)) * 1.0f / (BattleStageManager.Instance.laneCount - 1) * (6 - 1) + 0.5f) * -1 - 1;
                    }
                    else
                    {
                        //一行下に移動
                        cursol -= 1;
                    }
                }
                else
                {
                    print("error! invalid cursol");
                }
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                break;
            case StrategyState.Merging:
                //マージ中の画面
                break;
            case StrategyState.Recruiting:
                //採用中の画面
                break;
        }

    }
}
