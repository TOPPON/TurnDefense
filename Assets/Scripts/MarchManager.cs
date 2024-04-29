using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchManager : MonoBehaviour
{
    public static MarchManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    int lanesCursol = 1;
    int massCursol = BattleStageManager.Instance.laneLength + 2;

    //行動済みの範囲を抑えるためだけのリスト
    List<int> frontlinePlanningList = new List<int>();
    // Update is called once per frame
    void Update()
    {
    }
    public void StartMarchPlan()
    {
        lanesCursol = 1;
        massCursol = BattleStageManager.Instance.laneLength + 2;
        frontlinePlanningList.Clear();
        for (int i = 0; i < (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount); i++)
        {
            frontlinePlanningList.Add(0);
        }
    }
    public void UpdateMarchPlan()
    {
        //上から順に確認して行動を決めていく
        lanesCursol++;
        if (lanesCursol > BattleStageManager.Instance.laneCount)
        {
            lanesCursol -= BattleStageManager.Instance.laneCount;
            massCursol--;
            if (massCursol < 1)
            {
                GameManager.Instance.CompleteMarchPlan();
            }
        }
        int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
        Character target = BattleStageManager.Instance.frontline[frontlineIndex];
        DecideNextAction(target);
    }
    public void DecideNextAction(Character target)
    {
        if (target.exists == false) return;
        if (target.charaState == Character.CharaState.Enemy) return;
        //行動を決める。行動は4種類で、Attack,March,Skill,Waiting。
        //基本的にAttack＞March＞Waiting、Skillは悩む
        int lane = target.lane;
        int mass = target.mass;
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);
        //二ます先までに敵がいたらAttack
        if (mass >= BattleStageManager.Instance.laneLength + 2)//ゴールますと一致の場合、Attackはない
        {

        }
        else if (mass + 1 >= BattleStageManager.Instance.laneLength + 2)//ゴールますより一マス下の場合
        {
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                if (frontlinePlanningList[up1MassIndex] == 0 && frontlinePlanningList[targetFrontlineIndex] == 0)
                {
                    frontlinePlanningList[up1MassIndex] = 1;
                    frontlinePlanningList[targetFrontlineIndex] = 1;
                    target.nextAction = Character.CharaAction.Attack;
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//敵も攻撃予定
                    return;
                }
            }
        }
        else
        {
            //一マス上に敵がいる場合
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                if (frontlinePlanningList[up1MassIndex] == 0 && frontlinePlanningList[targetFrontlineIndex] == 0)
                {
                    frontlinePlanningList[up1MassIndex] = 1;
                    frontlinePlanningList[targetFrontlineIndex] = 1;
                    target.nextAction = Character.CharaAction.Attack;
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//敵も攻撃予定
                    return;
                }
            }

            //二マス上に敵がいる場合
            int up2MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 2);
            Character up2MassCharacter = BattleStageManager.Instance.frontline[up2MassIndex];
            if (up2MassCharacter.exists && up2MassCharacter.charaState == Character.CharaState.Enemy)
            {
                if (frontlinePlanningList[up1MassIndex] == 0 && frontlinePlanningList[up2MassIndex] == 0 && frontlinePlanningList[targetFrontlineIndex] == 0)
                {
                    frontlinePlanningList[up1MassIndex] = 1;
                    frontlinePlanningList[up2MassIndex] = 1;
                    frontlinePlanningList[targetFrontlineIndex] = 1;
                    target.nextAction = Character.CharaAction.Attack;
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//敵も攻撃予定
                    return;
                }
            }
        }
        //移動しようと思っている先がfrontlinePlanningListに含まれていなかったらAhead

        int aheadPlanMass = mass + 1;
        int aheadPlanMassIndex= BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, aheadPlanMass);
        if(frontlinePlanningList[aheadPlanMassIndex] == 0)
        {
            frontlinePlanningList[aheadPlanMassIndex] = 2;
            target.nextAction = Character.CharaAction.Ahead;
        }

        //スキルを発動するという手もありますね

    }
    public void DoAhead(Character target)
    {
        //歩いたので完了にする
        target.nextAction = Character.CharaAction.None;
    }
    public void DoSkill(Character target)
    {

    }
    public void DoAttack(Character target)
    {
    }
}
