using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruiteManager : MonoBehaviour
{
    public static RecruiteManager Instance;
    public enum RecruiteState
    {
        BeforeStatus,
        StatusRolling,
        AfterStatus,
        BeforeSkill,
        SkillRolling,
        AfterSkill
    }
    public RecruiteState recruiteState;
    public float statusAngle;
    public float skillAngle;
    public float statusRollingSpeed;
    public float skillRollingSpeed;
    public int statusRollingCount;
    public int skillRollingCount;
    public float stateTimer;
    public int[] statusRouletteNumber = new int[24];
    public int[] skillRouletteNumber = new int[4];
    public int hpSpots;
    public int atkSpots;
    public int spdSpots;
    public int skillSpots;
    public int skillAType;
    public int skillBType;
    public int skillCType;
    public int skillDType;
    public int skillEType;//E が存在する場合のみ
    int skillRouletteSpots;//4か5の想定
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void Activate()
    {
        recruiteState = RecruiteState.BeforeStatus;
        statusRollingCount = 1000;
        skillRollingCount = 1000;
        statusAngle = 0;
        skillAngle = 0;
        RecruiteDisplayManager.Instance.UpdateStatusArrow(statusAngle);
        RecruiteDisplayManager.Instance.UpdateSkillArrow(skillAngle);
        RecruiteDisplayManager.Instance.ActivateDisplay();

        //Todo:レジェンド対応
        skillRouletteSpots = 4;
        hpSpots = 3;
        atkSpots = 6;
        spdSpots = 8;
        skillSpots = 7;
        RecruiteDisplayManager.Instance.SetStatusRouletteWheel(hpSpots, atkSpots, spdSpots, skillSpots);
        skillAType = 1;
        skillBType = 2;
        skillCType = 3;
        skillDType = 4;
        RecruiteDisplayManager.Instance.SetSkillRouletteWheel(skillAType, skillBType, skillCType, skillDType);
    }

    // Update is called once per frame
    void Update()
    {
        //Todo: Update が RecruiteState の時だけ動くようにする
        switch (recruiteState)
        {
            case RecruiteState.BeforeStatus:
                statusRollingSpeed = Random.Range(10f, 15f);
                recruiteState = RecruiteState.StatusRolling;
                RecruiteDisplayManager.Instance.UpdateNextButtonText("スキップ");
                break;
            case RecruiteState.StatusRolling:
                if (statusRollingCount > 0)
                {
                    statusRollingCount--;
                    statusAngle += statusRollingSpeed;
                    RecruiteDisplayManager.Instance.UpdateStatusArrow(statusAngle);
                    statusRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                else
                {
                    recruiteState = RecruiteState.AfterStatus;
                }
                break;
            case RecruiteState.AfterStatus:
                recruiteState = RecruiteState.BeforeSkill;
                break;
            case RecruiteState.BeforeSkill:
                skillRollingSpeed = Random.Range(10f, 15f);
                recruiteState = RecruiteState.SkillRolling;
                break;
            case RecruiteState.SkillRolling:
                if (skillRollingCount > 0)
                {
                    skillRollingCount--;
                    skillAngle += skillRollingSpeed;
                    RecruiteDisplayManager.Instance.UpdateSkillArrow(skillAngle);
                    skillRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                else
                {
                    recruiteState = RecruiteState.AfterSkill;
                    RecruiteDisplayManager.Instance.UpdateNextButtonText("OK");
                }
                break;
            case RecruiteState.AfterSkill:
                break;
        }
    }
    public void PushAButton()
    {
        switch (recruiteState)
        {
            case RecruiteState.BeforeStatus:
                statusRollingSpeed = Random.Range(10f, 15f);
                recruiteState = RecruiteState.AfterStatus;
                for (int i = 0; i < statusRollingCount; i++)
                {
                    statusAngle += statusRollingSpeed;
                    statusRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                RecruiteDisplayManager.Instance.UpdateStatusArrow(statusAngle);
                break;
            case RecruiteState.StatusRolling:
                recruiteState = RecruiteState.AfterStatus;
                for (int i = 0; i < statusRollingCount; i++)
                {
                    statusAngle += statusRollingSpeed;
                    statusRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                RecruiteDisplayManager.Instance.UpdateStatusArrow(statusAngle);
                break;
            case RecruiteState.AfterStatus:
                break;
            case RecruiteState.BeforeSkill:
                recruiteState = RecruiteState.AfterSkill;
                RecruiteDisplayManager.Instance.UpdateNextButtonText("OK");
                skillRollingSpeed = Random.Range(10f, 15f);
                for (int i = 0; i < skillRollingCount; i++)
                {
                    skillAngle += skillRollingSpeed;
                    skillRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                RecruiteDisplayManager.Instance.UpdateSkillArrow(skillAngle);
                break;
            case RecruiteState.SkillRolling:
                recruiteState = RecruiteState.AfterSkill;
                RecruiteDisplayManager.Instance.UpdateNextButtonText("OK");
                for (int i = 0; i < skillRollingCount; i++)
                {
                    skillAngle += skillRollingSpeed;
                    skillRollingSpeed *= Random.Range(0.988f, 0.992f);
                }
                RecruiteDisplayManager.Instance.UpdateSkillArrow(skillAngle);
                break;
            case RecruiteState.AfterSkill:
                //キャラクターを追加する処理
                Character result = new Character();
                result.maxHp = 1; // 最大体力
                result.nowHp = result.maxHp; // 現在の体力
                result.power = 1; // 攻撃力
                result.attackSpd = 4; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
                result.skillType = 1; // スキルの種類
                                      //result.skillTurn; // スキルのリロード
                                      //result.skillnowTurn; // スキルの残りターン数。
                result.skillPoint = 0; // スキルポイント、０～１６で表現
                result.skillLevel = result.skillPoint / 3; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
                result.rarity = 1;
                result.reviveMaxTurn = result.rarity + 1;
                result.reviveTurn = 0;// 

                //追加ステータスを決める
                float resultStatusAngle = 360 - statusAngle % 360;
                int resultStatusArrowIndex = (int)(resultStatusAngle / 15);//0~23

                if (resultStatusArrowIndex < hpSpots)// hp穴
                {
                    result.maxHp = 2;
                    result.nowHp = result.maxHp;
                }
                else if (resultStatusArrowIndex < hpSpots + atkSpots)// atk穴
                {
                    result.power = 2;
                }
                else if (resultStatusArrowIndex < hpSpots + atkSpots + spdSpots) // spd穴
                {
                    result.attackSpd = 5;
                }
                else //スキル穴
                {
                    result.skillPoint = 1;
                }

                float resultSkillAngle = 360 - skillAngle % 360;
                int resultSkillArrowIndex = (int)(resultSkillAngle / (360 / skillRouletteSpots));//0~3
                switch (resultSkillArrowIndex)
                {
                    case 0:
                        result.skillType = skillAType;
                        break;
                    case 1:
                        result.skillType = skillBType;
                        break;
                    case 2:
                        result.skillType = skillCType;
                        break;
                    case 3:
                        result.skillType = skillDType;
                        break;
                    case 4:
                        result.skillType = skillEType;
                        break;
                }

                BattleStageManager.Instance.AddCharacterToCamp(result);

                //元の画面に戻す
                StrategyManager.Instance.FinishRecruite();
                RecruiteDisplayManager.Instance.DeactivateDisplay();
                break;
        }
    }
    public void PushNextButton()
    {
        PushAButton();
    }
}
