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
    int cursol = -1; // -1����-18 �L�����v -19 ���p -20 ��W -21 GO 1����Lanes*LaneLength ���
    int normalCursol = 1;//�ʏ��Ԃ̃J�[�\��
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
        //cursol = normalCursol;
        //BattleStageDisplayManager.Instance.UpdateCursol(cursol);
        BattleStageDisplayManager.Instance.DeactivateNormalCursol();
    }
    public void FinishRecruite()
    {
        strategyState = StrategyState.Normal;
    }
    public void FinishHaving()
    {
        strategyState = StrategyState.Normal;
        //cursol = normalCursol;
        //BattleStageDisplayManager.Instance.UpdateCursol(cursol);
        BattleStageDisplayManager.Instance.DeactivateNormalCursol();
    }
    public int CalcSellPrice(Character target)
    {
        print("CalcSellPrice:" + target.rarity * 5);
        return target.rarity * 5;
    }
    //����{�^�����������Ƃ��̃A�N�V����
    public void PushAButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
                //�ʏ���
                if (cursol == -21)//GO
                {
                    GameManager.Instance.PushGOButton();
                }
                else if (cursol == -20)//��W
                {
                    if (GameManager.Instance.money >= 10 &&
                        BattleStageManager.Instance.CheckEnableAddCharacter())
                    {
                        GameManager.Instance.ReduceMoney(10);
                        strategyState = StrategyState.Recruiting;
                        RecruiteManager.Instance.Activate();
                    }
                }
                else if (cursol == -19)//���p
                {
                    //�������Ȃ�
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
                //�J�[�\����Ƀv���C���[������Ώ�ԕω��AHaving�Ɉȍ~���ăJ�[�\����ς���
                //��W�}�X�Ȃ��W����
                //GO�}�X�Ȃ�^�[����i�߂�
                break;
            case StrategyState.Having:
                if (cursol == -21)//GO
                {
                    //�������Ȃ�
                }
                else if (cursol == -20)//��W
                {
                    //�������Ȃ�
                }
                else if (cursol == -19)//���p
                {
                    //���p
                    if (normalCursol < 0)
                    {
                        int campIndex = -normalCursol - 1;
                        int income = CalcSellPrice(BattleStageManager.Instance.camp[campIndex]);
                        GameManager.Instance.AddMoney(income);
                        BattleStageManager.Instance.RemoveCharacter(campIndex, -1);
                        FinishHaving();
                        cursol = normalCursol;
                        BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                    }
                    else if (normalCursol > 0)//�o�w�Ώۂ̂�Ȃ̂ŕs�ł����������A�\�ɂ���ꍇ�̓L�����v������
                    {
                        int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByCursol(cursol);
                        int income = CalcSellPrice(BattleStageManager.Instance.frontline[frontlineIndex]);
                        GameManager.Instance.AddMoney(income);
                        BattleStageManager.Instance.RemoveCharacter(-1, frontlineIndex);
                        FinishHaving();
                    }
                }
                else if (cursol < 0)
                {
                    int campIndex = BattleStageManager.Instance.GetCampIndexByCursol(cursol);
                    //�󂫂܂��Ȃ�ړ�������
                    Character target = BattleStageManager.Instance.camp[campIndex];
                    if (!target.exists)
                    {
                        Character normalCursolChara;
                        if (normalCursol > 0)//����̏ꍇ
                        {
                            //������̃L�����N�^�[�擾
                            int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByCursol(normalCursol);
                            normalCursolChara = BattleStageManager.Instance.frontline[frontlineIndex];
                            //�L�����v�ő҂��Ă���l���폜����K�v������
                            int frontlineWaitingIndex = BattleStageManager.Instance.GetCampIndexByEncampment(normalCursolChara.encampment);
                            //Character normalCursolWaitingChara = BattleStageManager.Instance.camp[frontlineWaitingIndex];
                            BattleStageManager.Instance.RemoveCharacter(frontlineWaitingIndex, -1);
                            BattleStageManager.Instance.AddCharacterToCamp(normalCursolChara, campIndex);
                            BattleStageManager.Instance.RemoveCharacter(-1, frontlineIndex);
                            FinishHaving();
                        }
                        else //�L�����v�̏ꍇ
                        {
                            int normalCampIndex = BattleStageManager.Instance.GetCampIndexByCursol(normalCursol);
                            normalCursolChara = BattleStageManager.Instance.camp[normalCampIndex];
                            BattleStageManager.Instance.AddCharacterToCamp(normalCursolChara, campIndex);
                            BattleStageManager.Instance.RemoveCharacter(normalCampIndex, -1);
                            FinishHaving();
                        }
                    }
                    else // �N���������ꍇ
                    {
                        if (cursol == normalCursol) break;
                        Character normalCursolChara;
                        Character havingChara= BattleStageManager.Instance.camp[campIndex];
                        if (normalCursol < 0) //�L�����v�̏ꍇ
                        {
                            int normalCampIndex = BattleStageManager.Instance.GetCampIndexByCursol(normalCursol);
                            normalCursolChara = BattleStageManager.Instance.camp[normalCampIndex];
                            if (havingChara.rarity == normalCursolChara.rarity && havingChara.rarity <= 4)
                            {
                                //�}�[�W��ʂ�
                                MergeManager.Instance.Activate(normalCursolChara, havingChara);
                                strategyState = StrategyState.Merging;
                            }
                        }
                    }
                    //����ȊO�͉������Ȃ�
                }
                else if (cursol > 0)
                {
                    //�󂫂܂��Ȃ�ړ�������
                    //�N�������牽�����Ȃ�
                }
                else
                {
                    print("error! cursol is invalid");
                }
                //�L�����N�^�[�������������
                //�����}�X�Ȃ�u��(�X�e�[�g������ύX����)
                //�}�[�W�\�ł���΃}�[�W��ʂ�
                //�󂫃}�X�Ȃ�ړ�������
                //���p�}�X�Ȃ甄��
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushAButton();
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                RecruiteManager.Instance.PushAButton();
                //�̗p���̉��
                break;
        }
    }
    public void PushBButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
                //�ʏ���
                /*strategyState = StrategyState.Merging;
                Character target1 = new Character();
                target1.maxHp = Random.Range(1, 8);
                target1.nowHp = target1.maxHp; // ���݂̗̑�
                target1.power = Random.Range(1, 8); // �U����
                target1.attackSpd = Random.Range(1, 8) + 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
                target1.skillType = Random.Range(1, 3); // �X�L���̎��
                target1.skillPoint = Random.Range(1, 8); // �X�L���|�C���g�A�O�`�P�U�ŕ\��
                target1.skillLevel = target1.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
                target1.rarity = Random.Range(1, 5);
                Character target2 = new Character();
                target2.maxHp = Random.Range(1, 8);
                target2.nowHp = target2.maxHp; // ���݂̗̑�
                target2.power = Random.Range(1, 8); // �U����
                target2.attackSpd = Random.Range(1, 8) + 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
                target2.skillType = Random.Range(1, 3); // �X�L���̎��
                target2.skillPoint = Random.Range(1, 8); // �X�L���|�C���g�A�O�`�P�U�ŕ\��
                target2.skillLevel = target2.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
                target2.rarity = Random.Range(1, 5);
                MergeManager.Instance.Activate(target1, target2);*/
                break;
            case StrategyState.Having:
                FinishHaving();
                cursol = normalCursol;
                BattleStageDisplayManager.Instance.UpdateCursol(cursol);
                //�L�����ƃJ�[�\����߂�
                //�L�����N�^�[�������������
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushBButton();
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }
    }

    //���{�^�����������Ƃ��̃A�N�V����
    public void PushLeftButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //�ʏ���
            case StrategyState.Having:
                //�L�����N�^�[�������������
                if (cursol < 0) //���w
                {
                    if (cursol >= -18)//�L�����v
                    {
                        if (cursol == -1 | cursol == -7 | cursol == -13)
                        {
                            //�L�����v���[�͔��p��
                            cursol = -19;
                        }
                        else
                        {
                            cursol += 1;
                        }
                    }
                    else if (cursol == -19) //���p
                    {
                        //�����Ȃ�
                    }
                    else if (cursol == -20) //��W
                    {
                        //�L�����v�̉E���Ɉړ�����
                        cursol = -18;
                    }
                    else if (cursol == -21) //GO
                    {
                        //��W�Ɉړ�����
                        cursol = -20;
                    }
                }
                else if (cursol > 0) //���
                {
                    if (cursol <= BattleStageManager.Instance.laneLength + 2)
                    {
                        //��ԍ��̗�͓����Ȃ�
                    }
                    else
                    {
                        //��񍶂Ɉړ�
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
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }
    }
    public void PushRightButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //�ʏ���
            case StrategyState.Having:
                //�L�����N�^�[�������������
                if (cursol < 0) //���w
                {
                    if (cursol >= -18)//�L�����v
                    {
                        if (cursol == -6 | cursol == -12 | cursol == -18)
                        {
                            //�L�����v�E�[�͕�W��
                            cursol = -20;
                        }
                        else
                        {
                            cursol -= 1;
                        }
                    }
                    else if (cursol == -19) //���p
                    {
                        //�L�����v�̍����Ɉړ�����
                        cursol = -13;
                    }
                    else if (cursol == -20) //��W
                    {
                        //GO�Ɉړ�����
                        cursol = -21;
                    }
                    else if (cursol == -21) //GO
                    {
                        //�ړ����Ȃ�
                    }
                }
                else if (cursol > 0) //���
                {
                    if (cursol > (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1))
                    {
                        //��ԉE�̗�͓����Ȃ�
                    }
                    else
                    {
                        //���E�Ɉړ�
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
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }
    }
    public void PushUpButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //�ʏ���
            case StrategyState.Having:
                //�L�����N�^�[�������������
                if (cursol < 0) //���w
                {
                    if (cursol >= -18)//�L�����v
                    {
                        if (cursol >= -6)
                        {
                            //�L�����v��[�͐����
                            cursol = (int)(((cursol * -1.0f) - 1) / (6 - 1) * (BattleStageManager.Instance.laneCount - 1) + 0.5f) * (BattleStageManager.Instance.laneLength + 2) + 1;
                        }
                        else
                        {
                            //�L�����v����s��Ɉړ�
                            cursol += 6;
                        }
                    }
                    else if (cursol == -19) //���p
                    {
                        //����̍����Ɉړ�
                        cursol = 1;
                    }
                    else if (cursol == -20) //��W
                    {
                        //����̉E���Ɉړ�
                        cursol = (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1) + 1;
                    }
                    else if (cursol == -21) //GO
                    {
                        //����̉E���Ɉړ�
                        cursol = (BattleStageManager.Instance.laneLength + 2) * (BattleStageManager.Instance.laneCount - 1) + 1;
                    }
                }
                else if (cursol > 0) //���
                {
                    if (cursol % (BattleStageManager.Instance.laneLength + 2) == 0)
                    {
                        //��ԏ�̗�͓����Ȃ�
                    }
                    else
                    {
                        //��s��Ɉړ�
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
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }

    }
    public void PushDownButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
            //�ʏ���
            case StrategyState.Having:
                //�L�����N�^�[�������������
                if (cursol < 0) //���w
                {
                    if (cursol >= -18)//�L�����v
                    {
                        if (cursol <= -13)
                        {
                            //�L�����v���[�͓����Ȃ�
                        }
                        else
                        {
                            //�L�����v����s���Ɉړ�
                            cursol -= 6;
                        }
                    }
                    else if (cursol == -19) //���p
                    {
                        //�����Ȃ�
                    }
                    else if (cursol == -20) //��W
                    {
                        //�����Ȃ�
                    }
                    else if (cursol == -21) //GO
                    {
                        //�����Ȃ�
                    }
                }
                else if (cursol > 0) //���
                {
                    if (cursol % (BattleStageManager.Instance.laneLength + 2) == 1)
                    {
                        //�L�����v�Ɉړ�
                        cursol = (int)((int)(cursol / (BattleStageManager.Instance.laneLength + 2)) * 1.0f / (BattleStageManager.Instance.laneCount - 1) * (6 - 1) + 0.5f) * -1 - 1;
                    }
                    else
                    {
                        //��s���Ɉړ�
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
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }

    }
    public void PushSellButton()
    {
        if (GameManager.Instance.turnState == GameManager.TurnState.Strategy && strategyState == StrategyState.Having)
        {
            //����ȍ~�͒ʏ�̔��p�Ɠ����ɂ��邱��
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
            else if (normalCursol > 0)//�o�w�Ώۂ̂�Ȃ̂ŕs�ł����������A�\�ɂ���ꍇ�̓L�����v������
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
    }
    public void PushRecruiteButton()
    {
        if (GameManager.Instance.turnState == GameManager.TurnState.Strategy && strategyState == StrategyState.Normal)
        {
            if (GameManager.Instance.money >= 10 &&
                BattleStageManager.Instance.CheckEnableAddCharacter())
            {
                GameManager.Instance.ReduceMoney(10);
                strategyState = StrategyState.Recruiting;
                RecruiteManager.Instance.Activate();
            }
        }
    }
}
