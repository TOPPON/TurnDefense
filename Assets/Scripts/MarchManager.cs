using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchManager : MonoBehaviour
{
    public static MarchManager Instance;
    public enum MarchState
    {
        Normal,
        AttackAnimation,
        SkillAnimation,
        Enemy
    }
    MarchState marchState;
    int lanesCursol;
    int massCursol;

    //s“®Ï‚İ‚Ì”ÍˆÍ‚ğ—}‚¦‚é‚½‚ß‚¾‚¯‚ÌƒŠƒXƒg
    List<int> frontlinePlanningList = new List<int>();
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
        massCursol = 1;//‰º‚©‚çŒ©‚Ä‚¢‚­
        marchState = MarchState.Enemy;
    }
    public void UpdateMarchPlan()
    {
        //ã‚©‚ç‡‚ÉŠm”F‚µ‚Äs“®‚ğŒˆ‚ß‚Ä‚¢‚­
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
                lanesCursol++;
                if (lanesCursol > BattleStageManager.Instance.laneCount)
                {
                    lanesCursol -= BattleStageManager.Instance.laneCount;
                    massCursol--;
                    if (massCursol < 1)
                    {
                        //“G‚ÌˆÚ“®‚ªƒXƒ^[ƒg
                        StartDoMarchEnemy();
                        return;
                    }
                }
                int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
                Character target = BattleStageManager.Instance.frontline[frontlineIndex];
                if (!target.exists) return;
                if (target.charaState == Character.CharaState.Enemy) return;
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
                break;
            case MarchState.AttackAnimation:
                AttackAnimationManager.Instance.UpdateAttackAnimation();
                break;
            case MarchState.Enemy:
                UpdateDoMarchEnemy();
                break;
        }
    }
    public void UpdateDoMarchEnemy()
    {
        lanesCursol++;
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
        if (!enemy.exists) return;
        if (enemy.charaState != Character.CharaState.Enemy) return;
        //ˆêƒ}ƒXæ‚Ìƒ}ƒX‚ğŠm”F‚µ‚Ä“®‚¯‚»‚¤‚È‚ç“®‚­
        int mass = enemy.mass;
        int lane = enemy.lane;
        if (mass <= 1) return;
        int aheadFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass - 1);
        Character AheadChara = BattleStageManager.Instance.frontline[aheadFrontlineIndex];
        if (!AheadChara.exists)
        {
            BattleStageManager.Instance.FrontlineCharacterMove(enemy, 0, -1);
        }
        /*switch (enemy.nextAction)
        {
            case Character.CharaAction.Attack:
                //‚±‚ê‚Í‘¶İ‚µ‚È‚¢‚Í‚¸
                print("error! invalid enemy nextAction: attack");
                enemy.nextAction = Character.CharaAction.None;
                break;
            case Character.CharaAction.Ahead:
                BattleStageManager.Instance.FrontlineCharacterMove(enemy, 0, -1);
                //•à‚¢‚½‚Ì‚ÅŠ®—¹‚É‚·‚é
                enemy.nextAction = Character.CharaAction.None;
                break;
            case Character.CharaAction.Waiting:
                enemy.nextAction = Character.CharaAction.None;
                break;
        }*/
    }
    public void DecideNextAction(Character target)
    {
        if (target.exists == false) return;
        if (target.charaState == Character.CharaState.Enemy) return;
        //s“®‚ğŒˆ‚ß‚éBs“®‚Í4í—Ş‚ÅAAttack,March,Skill,WaitingB
        //Šî–{“I‚ÉAttack„March„WaitingASkill‚Í”Y‚Ş
        int lane = target.lane;
        int mass = target.mass;
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);

        if (mass >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚Æˆê’v‚Ìê‡AAttack‚àAhead‚à‚È‚¢‚½‚ß‚¢‚Á‚½‚ñWaitingŠm’è
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

            // = FetchAheadEnemyCharacterIndex(lane, mass);
            print("enemyFrontlineIndex" + enemyFrontlineIndex);
            print(enemy);
            if (enemy.exists)
            {
                print("“G‚Á‚Û‚¢‚ÌƒLƒƒƒ‰‚Ì state :" + enemy.charaState);
                print("“G‚Á‚Û‚¢‚ÌƒLƒƒƒ‰‚Ì‘¶İ :" + enemy.exists);
            }

            if (enemy.exists)
            {
                print("null‚¶‚á‚È‚©‚Á‚½‚æ");
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
                        enemy.nextAction = Character.CharaAction.Attack;//“G‚àUŒ‚—\’è
                        print("UŒ‚2—\’è");
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
                        enemy.nextAction = Character.CharaAction.Attack;//“G‚àUŒ‚—\’è
                        print("UŒ‚1—\’è");
                        return;
                    }
                }
                print("“–‚Ä‚Í‚Ü‚ç‚È‚©‚Á‚½");
            }
        }
        print("UŒ‚‚Í‚È‚µ");
        /*
        //“ñ‚Ü‚·æ‚Ü‚Å‚É“G‚ª‚¢‚½‚çAttack
        if (mass >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚Æˆê’v‚Ìê‡AAttack‚àAhead‚à‚È‚¢‚½‚ß‚¢‚Á‚½‚ñWaitingŠm’è
        {
            target.nextAction = Character.CharaAction.Waiting;
            frontlinePlanningList[targetFrontlineIndex] = 4;
            return;
        }
        else if (mass + 1 >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚æ‚èˆêƒ}ƒX‰º‚Ìê‡
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
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//“G‚àUŒ‚—\’è
                    return;
                }
            }
        }
        else
        {
            //ˆêƒ}ƒXã‚É“G‚ª‚¢‚éê‡
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                if (frontlinePlanningList[up1MassIndex] == 0 && frontlinePlanningList[targetFrontlineIndex] == 0)
                {
                    frontlinePlanningList[up1MassIndex] = 1;
                    frontlinePlanningList[targetFrontlineIndex] = 1;
                    target.nextAction = Character.CharaAction.Attack;
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//“G‚àUŒ‚—\’è
                    return;
                }
            }

            //“ñƒ}ƒXã‚É“G‚ª‚¢‚éê‡
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
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//“G‚àUŒ‚—\’è
                    return;
                }
            }
        }*/
        //ˆÚ“®‚µ‚æ‚¤‚Æv‚Á‚Ä‚¢‚éæ‚ªfrontlinePlanningList‚ÉŠÜ‚Ü‚ê‚Ä‚¢‚È‚©‚Á‚½‚çAhead

        int aheadPlanMass = mass + 1;
        int aheadPlanMassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, aheadPlanMass);
        if (frontlinePlanningList[aheadPlanMassIndex] == 0)
        {
            frontlinePlanningList[aheadPlanMassIndex] = 2;
            target.nextAction = Character.CharaAction.Ahead;
            return;
        }

        //ƒXƒLƒ‹‚ğ”­“®‚·‚é‚Æ‚¢‚¤è‚à‚ ‚è‚Ü‚·‚Ë


        //‰½‚à‚È‚©‚Á‚½‚ç‘Ò‹@
        target.nextAction = Character.CharaAction.Waiting;
        frontlinePlanningList[targetFrontlineIndex] = 4;
    }
    public void DoAhead(Character target)
    {
        BattleStageManager.Instance.FrontlineCharacterMove(target);
        //•à‚¢‚½‚Ì‚ÅŠ®—¹‚É‚·‚é
        target.nextAction = Character.CharaAction.None;
    }
    public void DoSkill(Character target)
    {

    }
    public void DoAttack(Character target)
    {
        marchState = MarchState.AttackAnimation;
        int enemyFrontlineIndex = FetchAheadEnemyCharacterIndex(target.lane, target.mass);
        if (enemyFrontlineIndex == -1)
        {
            print("error! enemy not found");
            return;
        }
        Character enemy = BattleStageManager.Instance.frontline[enemyFrontlineIndex];
        AttackAnimationManager.Instance.Activate(target, enemy);
        enemy.nextAction = Character.CharaAction.None;
        target.nextAction = Character.CharaAction.None;
    }

    public int FetchAheadEnemyCharacterIndex(int lane, int mass)
    {
        print("lane:" + lane + "mass:" + mass);
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);

        if (mass >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚Æˆê’v‚Ìê‡AAttack‚àAhead‚à‚È‚¢‚½‚ß‚¢‚Á‚½‚ñWaitingŠm’è
        {
            print("error! invalid mass:" + mass);
            return -1;
        }
        else if (mass + 1 >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚æ‚èˆêƒ}ƒX‰º‚Ìê‡
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
            print("316");
            //ˆêƒ}ƒXã‚É“G‚ª‚¢‚éê‡
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            print("320" + up1MassIndex);
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                print("323");
                return up1MassIndex;
            }

            //“ñƒ}ƒXã‚É“G‚ª‚¢‚éê‡
            int up2MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 2);
            Character up2MassCharacter = BattleStageManager.Instance.frontline[up2MassIndex];
            print("2ƒ}ƒXæ‚ÌƒLƒƒƒ‰‚Ì state :" + up2MassCharacter.charaState);
            print("2ƒ}ƒXæ‚ÌƒLƒƒƒ‰‚Ì‘¶İ :" + up2MassCharacter.exists);
            print("330" + up2MassIndex);
            if (up2MassCharacter.exists && up2MassCharacter.charaState == Character.CharaState.Enemy)
            {
                print("333");
                return up2MassIndex;
            }
        }
        print("337");
        return -1;
    }

    public void FinishAttackAnimation()
    {
        if (marchState == MarchState.AttackAnimation)
        {
            marchState = MarchState.Normal;
            AttackAnimationManager.Instance.Deactivate();
        }
        else
        {
            print("error! invalid marchState:" + marchState);
        }
    }
}
