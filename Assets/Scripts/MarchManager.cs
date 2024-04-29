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

    //s“®Ï‚İ‚Ì”ÍˆÍ‚ğ—}‚¦‚é‚½‚ß‚¾‚¯‚ÌƒŠƒXƒg
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
        //ã‚©‚ç‡‚ÉŠm”F‚µ‚Äs“®‚ğŒˆ‚ß‚Ä‚¢‚­
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
        //s“®‚ğŒˆ‚ß‚éBs“®‚Í4í—Ş‚ÅAAttack,March,Skill,WaitingB
        //Šî–{“I‚ÉAttack„March„WaitingASkill‚Í”Y‚Ş
        int lane = target.lane;
        int mass = target.mass;
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);
        //“ñ‚Ü‚·æ‚Ü‚Å‚É“G‚ª‚¢‚½‚çAttack
        if (mass >= BattleStageManager.Instance.laneLength + 2)//ƒS[ƒ‹‚Ü‚·‚Æˆê’v‚Ìê‡AAttack‚Í‚È‚¢
        {

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
        }
        //ˆÚ“®‚µ‚æ‚¤‚Æv‚Á‚Ä‚¢‚éæ‚ªfrontlinePlanningList‚ÉŠÜ‚Ü‚ê‚Ä‚¢‚È‚©‚Á‚½‚çAhead

        int aheadPlanMass = mass + 1;
        int aheadPlanMassIndex= BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, aheadPlanMass);
        if(frontlinePlanningList[aheadPlanMassIndex] == 0)
        {
            frontlinePlanningList[aheadPlanMassIndex] = 2;
            target.nextAction = Character.CharaAction.Ahead;
        }

        //ƒXƒLƒ‹‚ğ”­“®‚·‚é‚Æ‚¢‚¤è‚à‚ ‚è‚Ü‚·‚Ë

    }
    public void DoAhead(Character target)
    {
        //•à‚¢‚½‚Ì‚ÅŠ®—¹‚É‚·‚é
        target.nextAction = Character.CharaAction.None;
    }
    public void DoSkill(Character target)
    {

    }
    public void DoAttack(Character target)
    {
    }
}
