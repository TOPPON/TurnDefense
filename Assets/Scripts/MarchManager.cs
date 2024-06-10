using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchManager : MonoBehaviour
{
    public static MarchManager Instance;
    public enum MarchState
    {
        Normal,
        BeforeAttack,
        AttackAnimation,
        AfterAttack,
        SkillAnimation,
        Enemy
    }
    MarchState marchState;
    int lanesCursol;
    int massCursol;
    float marchTimer = 0;
    public const float MARCH_INTERVAL = 0.2f;

    //行動済みの範囲を抑えるためだけのリスト
    List<int> frontlinePlanningList = new List<int>();

    //アニメーション前に一時的に対象のキャラを保存する用
    Character enemyForAttackAnimation;
    Character allyForAttackAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void StartMarchPlan()
    {
        lanesCursol = 0;
        massCursol = BattleStageManager.Instance.laneLength + 2;
        frontlinePlanningList.Clear();
        for (int i = 0; i < (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount); i++)
        {
            frontlinePlanningList.Add(0);
        }
    }
    public void StartDoMarch()
    {
        lanesCursol = 0;
        massCursol = BattleStageManager.Instance.laneLength + 2;
        marchState = MarchState.Normal;
    }
    public void StartDoMarchEnemy()
    {
        lanesCursol = 0;
        massCursol = 1;//下から見ていく
        marchState = MarchState.Enemy;
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
                return;
            }
        }
        int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
        Character target = BattleStageManager.Instance.frontline[frontlineIndex];
        DecideNextAction(target);
    }
    public void UpdateMarch()
    {
        switch (marchState)
        {
            case MarchState.Normal:
                marchTimer += Time.deltaTime;
                if (marchTimer > MARCH_INTERVAL)
                {
                    marchTimer = 0;
                    lanesCursol++;
                    if (lanesCursol > BattleStageManager.Instance.laneCount)
                    {
                        lanesCursol -= BattleStageManager.Instance.laneCount;
                        massCursol--;
                        if (massCursol < 1)
                        {
                            //敵の移動がスタート
                            StartDoMarchEnemy();
                            return;
                        }
                    }
                    int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
                    Character target = BattleStageManager.Instance.frontline[frontlineIndex];
                    if (!target.exists)
                    {
                        marchTimer = MARCH_INTERVAL;
                        return;
                    }
                    if (target.charaState == Character.CharaState.Enemy)
                    {
                        marchTimer = MARCH_INTERVAL;
                        return;
                    }
                    switch (target.nextAction)
                    {
                        case Character.CharaAction.Attack:
                            DoAttack(target);
                            break;
                        case Character.CharaAction.Ahead:
                            DoAhead(target);
                            break;
                        case Character.CharaAction.Waiting:
                            target.nextAction = Character.CharaAction.None;
                            break;
                    }
                }
                break;
            case MarchState.BeforeAttack:
                marchTimer += Time.deltaTime;
                if (marchTimer > 1f)
                {
                    marchTimer = 0;
                    marchState = MarchState.AttackAnimation;
                    AttackAnimationManager.Instance.Activate(allyForAttackAnimation, enemyForAttackAnimation);
                }
                break;
            case MarchState.AttackAnimation:
                AttackAnimationManager.Instance.UpdateAttackAnimation();
                break;
            case MarchState.AfterAttack:
                marchTimer += Time.deltaTime;
                if (marchTimer > 1f)
                {
                    marchTimer = 0;
                    marchState = MarchState.Normal;
                }
                break;
            case MarchState.Enemy:
                UpdateDoMarchEnemy();
                break;
        }
    }
    public void UpdateDoMarchEnemy()
    {
        marchTimer += Time.deltaTime;
        if (marchTimer > MARCH_INTERVAL)
        {
            lanesCursol++;
            marchTimer = 0;
            if (lanesCursol > BattleStageManager.Instance.laneCount)
            {
                lanesCursol -= BattleStageManager.Instance.laneCount;
                massCursol++;
                if (massCursol > BattleStageManager.Instance.laneLength + 2)
                {
                    GameManager.Instance.CompleteDoMarch();
                    return;
                }
            }
            int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
            Character enemy = BattleStageManager.Instance.frontline[frontlineIndex];
            if (!enemy.exists)
            {
                marchTimer = MARCH_INTERVAL;
                return;
            }
            if (enemy.charaState != Character.CharaState.Enemy)
            {
                marchTimer = MARCH_INTERVAL;
                return;
            }
            //一マス先のマスを確認して動けそうなら動く
            if (enemy.nextAction == Character.CharaAction.None)
            {
                marchTimer = MARCH_INTERVAL;
                return;
            }
            int mass = enemy.mass;
            int lane = enemy.lane;
            if (mass <= 1)
            {
                marchTimer = 0.1f;
                return;
            }
            int aheadFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass - 1);
            Character AheadChara = BattleStageManager.Instance.frontline[aheadFrontlineIndex];
            if (!AheadChara.exists)
            {
                BattleStageManager.Instance.FrontlineCharacterMove(enemy, 0, -1);
            }
        }
    }
    public void DecideNextAction(Character target)
    {
        if (target.exists == false) return;
        if (target.charaState == Character.CharaState.Enemy)
        {
            target.nextAction = Character.CharaAction.Ahead;
            return;
        }
        //行動を決める。行動は4種類で、Attack,March,Skill,Waiting。
        //基本的にAttack＞March＞Waiting、Skillは悩む
        int lane = target.lane;
        int mass = target.mass;
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);

        if (mass >= BattleStageManager.Instance.laneLength + 2)//ゴールますと一致の場合、AttackもAheadもないためいったんWaiting確定
        {
            target.nextAction = Character.CharaAction.Waiting;
            frontlinePlanningList[targetFrontlineIndex] = 4;
            return;
        }
        int enemyFrontlineIndex = FetchAheadEnemyCharacterIndex(lane, mass);
        Character enemy = null;
        if (enemyFrontlineIndex != -1)
        {
            enemy = BattleStageManager.Instance.frontline[enemyFrontlineIndex];

            if (enemy.exists)
            {
                int enemyLane = enemy.lane;
                int enemyMass = enemy.mass;
                if (enemyMass - mass == 2)
                {
                    int up1Index = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
                    int up2Index = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 2);
                    if (frontlinePlanningList[targetFrontlineIndex] == 0 && frontlinePlanningList[up1Index] == 0 && frontlinePlanningList[up2Index] == 0)
                    {
                        frontlinePlanningList[targetFrontlineIndex] = 1;
                        frontlinePlanningList[up1Index] = 1;
                        frontlinePlanningList[up2Index] = 1;
                        target.nextAction = Character.CharaAction.Attack;
                        enemy.nextAction = Character.CharaAction.Attack;//敵も攻撃予定
                        return;
                    }
                }
                if (enemyMass - mass == 1)
                {
                    int up1Index = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
                    if (frontlinePlanningList[targetFrontlineIndex] == 0 && frontlinePlanningList[up1Index] == 0)
                    {
                        frontlinePlanningList[targetFrontlineIndex] = 1;
                        frontlinePlanningList[up1Index] = 1;
                        target.nextAction = Character.CharaAction.Attack;
                        enemy.nextAction = Character.CharaAction.Attack;//敵も攻撃予定
                        return;
                    }
                }
            }
        }
        /*
        //二ます先までに敵がいたらAttack
        if (mass >= BattleStageManager.Instance.laneLength + 2)//ゴールますと一致の場合、AttackもAheadもないためいったんWaiting確定
        {
            target.nextAction = Character.CharaAction.Waiting;
            frontlinePlanningList[targetFrontlineIndex] = 4;
            return;
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
        }*/
        //移動しようと思っている先がfrontlinePlanningListに含まれていなかったらAhead

        int aheadPlanMass = mass + 1;
        int aheadPlanMassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, aheadPlanMass);
        if (frontlinePlanningList[aheadPlanMassIndex] == 0)
        {
            frontlinePlanningList[aheadPlanMassIndex] = 2;
            target.nextAction = Character.CharaAction.Ahead;
            return;
        }

        //スキルを発動するという手もありますね


        //何もなかったら待機
        target.nextAction = Character.CharaAction.Waiting;
        frontlinePlanningList[targetFrontlineIndex] = 4;
    }
    public void DoAhead(Character target)
    {
        BattleStageManager.Instance.FrontlineCharacterMove(target);
        //歩いたので完了にする
        target.nextAction = Character.CharaAction.None;
    }
    public void DoSkill(Character target)
    {

    }
    public void DoAttack(Character target)
    {
        marchState = MarchState.BeforeAttack;
        int enemyFrontlineIndex = FetchAheadEnemyCharacterIndex(target.lane, target.mass);
        if (enemyFrontlineIndex == -1)
        {
            print("error! enemy not found");
            return;
        }
        Character enemy = BattleStageManager.Instance.frontline[enemyFrontlineIndex];
        allyForAttackAnimation = target;
        enemyForAttackAnimation = enemy;
        //バトル開始のアニメーション
        BattleStageDisplayManager.Instance.OccurBattleSymbol(target.lane, target.mass, enemy.lane, enemy.mass);
        //
        enemy.nextAction = Character.CharaAction.None;
        target.nextAction = Character.CharaAction.None;
    }

    public int FetchAheadEnemyCharacterIndex(int lane, int mass)
    {
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);

        if (mass >= BattleStageManager.Instance.laneLength + 2)//ゴールますと一致の場合、AttackもAheadもないためいったんWaiting確定
        {
            print("error! invalid mass:" + mass);
            return -1;
        }
        else if (mass + 1 >= BattleStageManager.Instance.laneLength + 2)//ゴールますより一マス下の場合
        {
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                return up1MassIndex;
            }
        }
        else
        {
            //一マス上に敵がいる場合
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                return up1MassIndex;
            }

            //二マス上に敵がいる場合
            int up2MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 2);
            Character up2MassCharacter = BattleStageManager.Instance.frontline[up2MassIndex];
            if (up2MassCharacter.exists && up2MassCharacter.charaState == Character.CharaState.Enemy)
            {
                return up2MassIndex;
            }
        }
        return -1;
    }

    public void FinishAttackAnimation()
    {
        if (marchState == MarchState.AttackAnimation)
        {
            marchState = MarchState.AfterAttack;
            AttackAnimationManager.Instance.Deactivate();
        }
        else
        {
            print("error! invalid marchState:" + marchState);
        }
    }
}
