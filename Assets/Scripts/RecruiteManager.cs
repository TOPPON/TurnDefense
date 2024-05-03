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
    public int skillEType;//E �����݂���ꍇ�̂�
    int skillRouletteSpots;//4��5�̑z��
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

        //Todo:���W�F���h�Ή�
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
        //Todo: Update �� RecruiteState �̎����������悤�ɂ���
        switch (recruiteState)
        {
            case RecruiteState.BeforeStatus:
                statusRollingSpeed = Random.Range(10f, 15f);
                recruiteState = RecruiteState.StatusRolling;
                RecruiteDisplayManager.Instance.UpdateNextButtonText("�X�L�b�v");
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
                //�L�����N�^�[��ǉ����鏈��
                Character result = new Character();
                result.maxHp = 1; // �ő�̗�
                result.nowHp = result.maxHp; // ���݂̗̑�
                result.power = 1; // �U����
                result.attackSpd = 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
                result.skillType = 1; // �X�L���̎��
                                      //result.skillTurn; // �X�L���̃����[�h
                                      //result.skillnowTurn; // �X�L���̎c��^�[�����B
                result.skillPoint = 0; // �X�L���|�C���g�A�O�`�P�U�ŕ\��
                result.skillLevel = result.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
                result.rarity = 1;
                result.reviveMaxTurn = result.rarity + 1;
                result.reviveTurn = 0;// 

                //�ǉ��X�e�[�^�X�����߂�
                float resultStatusAngle = 360 - statusAngle % 360;
                int resultStatusArrowIndex = (int)(resultStatusAngle / 15);//0~23

                if (resultStatusArrowIndex < hpSpots)// hp��
                {
                    result.maxHp = 2;
                    result.nowHp = result.maxHp;
                }
                else if (resultStatusArrowIndex < hpSpots + atkSpots)// atk��
                {
                    result.power = 2;
                }
                else if (resultStatusArrowIndex < hpSpots + atkSpots + spdSpots) // spd��
                {
                    result.attackSpd = 5;
                }
                else //�X�L����
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

                //���̉�ʂɖ߂�
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
