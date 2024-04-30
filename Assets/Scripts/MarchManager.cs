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
    int lanesCursol;
    int massCursol;

    //�s���ς݂͈̔͂�}���邽�߂����̃��X�g
    List<int> frontlinePlanningList = new List<int>();
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
    }
    public void StartDoMarchEnemy()
    {
        lanesCursol = 0;
        massCursol = BattleStageManager.Instance.laneLength + 2;
    }
    public void UpdateMarchPlan()
    {
        //�ォ�珇�Ɋm�F���čs�������߂Ă���
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
    public void UpdateDoMarch()
    {
        //�ォ�珇�Ɋm�F���čs�������߂Ă���
        lanesCursol++;
        if (lanesCursol > BattleStageManager.Instance.laneCount)
        {
            lanesCursol -= BattleStageManager.Instance.laneCount;
            massCursol--;
            if (massCursol < 1)
            {
                GameManager.Instance.CompleteDoMarch();
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
    }
    public void UpdateDoMarchEnemy()
    {
        //�ォ�珇�Ɋm�F���čs�������߂Ă���
        lanesCursol++;
        if (lanesCursol > BattleStageManager.Instance.laneCount)
        {
            lanesCursol -= BattleStageManager.Instance.laneCount;
            massCursol--;
            if (massCursol < 1)
            {
                GameManager.Instance.CompleteDoMarchEnemy();
                return;
            }
        }
        int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lanesCursol, massCursol);
        Character enemy = BattleStageManager.Instance.frontline[frontlineIndex];
        if (!enemy.exists) return;
        if (enemy.charaState != Character.CharaState.Enemy) return;
        switch (enemy.nextAction)
        {
            case Character.CharaAction.Attack:
                break;
            case Character.CharaAction.Ahead:
                break;
            case Character.CharaAction.Waiting:
                break;
        }
    }
    public void DecideNextAction(Character target)
    {
        if (target.exists == false) return;
        if (target.charaState == Character.CharaState.Enemy) return;
        //�s�������߂�B�s����4��ނŁAAttack,March,Skill,Waiting�B
        //��{�I��Attack��March��Waiting�ASkill�͔Y��
        int lane = target.lane;
        int mass = target.mass;
        int targetFrontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass);
        //��܂���܂łɓG��������Attack
        if (mass >= BattleStageManager.Instance.laneLength + 2)//�S�[���܂��ƈ�v�̏ꍇ�AAttack��Ahead���Ȃ����߂�������Waiting�m��
        {
            target.nextAction = Character.CharaAction.Waiting;
            frontlinePlanningList[targetFrontlineIndex] = 4;
            return;
        }
        else if (mass + 1 >= BattleStageManager.Instance.laneLength + 2)//�S�[���܂�����}�X���̏ꍇ
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
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//�G���U���\��
                    return;
                }
            }
        }
        else
        {
            //��}�X��ɓG������ꍇ
            int up1MassIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, mass + 1);
            Character up1MassCharacter = BattleStageManager.Instance.frontline[up1MassIndex];
            if (up1MassCharacter.exists && up1MassCharacter.charaState == Character.CharaState.Enemy)
            {
                if (frontlinePlanningList[up1MassIndex] == 0 && frontlinePlanningList[targetFrontlineIndex] == 0)
                {
                    frontlinePlanningList[up1MassIndex] = 1;
                    frontlinePlanningList[targetFrontlineIndex] = 1;
                    target.nextAction = Character.CharaAction.Attack;
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//�G���U���\��
                    return;
                }
            }

            //��}�X��ɓG������ꍇ
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
                    up1MassCharacter.nextAction = Character.CharaAction.Attack;//�G���U���\��
                    return;
                }
            }
        }
        //�ړ����悤�Ǝv���Ă���悪frontlinePlanningList�Ɋ܂܂�Ă��Ȃ�������Ahead

        int aheadPlanMass = mass + 1;
        int aheadPlanMassIndex= BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, aheadPlanMass);
        if(frontlinePlanningList[aheadPlanMassIndex] == 0)
        {
            frontlinePlanningList[aheadPlanMassIndex] = 2;
            target.nextAction = Character.CharaAction.Ahead;
            return;
        }

        //�X�L���𔭓�����Ƃ����������܂���


        //�����Ȃ�������ҋ@
        target.nextAction = Character.CharaAction.Waiting;
        frontlinePlanningList[targetFrontlineIndex] = 4;
    }
    public void DoAhead(Character target)
    {
        BattleStageManager.Instance.FrontlineCharacterMove(target);
        //�������̂Ŋ����ɂ���
        target.nextAction = Character.CharaAction.None;
    }
    public void DoSkill(Character target)
    {

    }
    public void DoAttack(Character target)
    {
        target.nextAction = Character.CharaAction.None;
    }
}
