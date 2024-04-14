using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;
    int cursol;//0:実行 1:戻る
    Character target1;
    Character target2;
    Character result;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void Activate(Character target1, Character target2)
    {
        this.target1 = target1;
        this.target2 = target2;
        this.result = CalcResultCharacter(target1, target2);
        cursol = 0;
        MergeDisplayManager.Instance.UpdateCursol(cursol);
        MergeDisplayManager.Instance.ActivateDisplay();
        MergeDisplayManager.Instance.SetTarget1Character(target1);
        MergeDisplayManager.Instance.SetTarget2Character(target2);
        MergeDisplayManager.Instance.SetResultCharacter(result, target1.skillType, target2.skillType);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void PushMergeDoButton()
    {
        DoMerge();
    }
    public void PushMergeBackButton()
    {
        Back();
    }
    public void PushAButton()
    {
        switch (cursol)
        {
            case 0:
                DoMerge();
                break;
            case 1:
                Back();
                break;
        }
    }
    public void PushBButton()
    {
        cursol = 1;
        MergeDisplayManager.Instance.UpdateCursol(cursol);
    }
    public void PushLeftButton()
    {
        cursol = 1;
        MergeDisplayManager.Instance.UpdateCursol(cursol);
    }
    public void PushRightButton()
    {
        cursol = 0;
        MergeDisplayManager.Instance.UpdateCursol(cursol);
    }
    void DoMerge()
    {
        //マージする処理
        StrategyManager.Instance.FinishMerge();
        MergeDisplayManager.Instance.DeactivateDisplay();
        int skillResult = Random.Range(0, 2);
        switch (skillResult)
        {
            case 0:
                result.skillType = target1.skillType;//ターンの処理とか追加する必要がありそう
                break;
            case 1:
                result.skillType = target2.skillType;
                break;
        }
        BattleStageManager.Instance.AddCharacterToCamp(result);
    }
    void Back()
    {
        StrategyManager.Instance.FinishMerge();
        MergeDisplayManager.Instance.DeactivateDisplay();
    }
    Character CalcResultCharacter(Character target1, Character target2)
    {
        Character result = new Character();
        result.maxHp = target1.maxHp + target2.maxHp - 1; // 最大体力
        result.nowHp = result.maxHp; // 現在の体力
        result.power = target1.power + target2.power - 1; // 攻撃力
        result.attackSpd = target1.attackSpd + target2.attackSpd - 4; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
        //result.skillType; // スキルの種類
        //result.skillTurn; // スキルのリロード
        //result.skillnowTurn; // スキルの残りターン数。
        result.skillPoint = target1.skillPoint + target2.skillPoint; // スキルポイント、０～１６で表現
        result.skillLevel = result.skillPoint / 3; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
        result.rarity = target1.rarity + 1;
        result.reviveMaxTurn = result.rarity + 1;
        result.reviveTurn = 0;// Random.Range(0, result.reviveMaxTurn+1);//デバッグ用
        return result;
    }
}
