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
    int cursol = 1; // -1����-18 �L�����v -19 ���p -20 ��W -21 GO 1����Lanes*LaneLength ���
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

    //����{�^�����������Ƃ��̃A�N�V����
    public void PushAButton()
    {
        switch (strategyState)
        {
            case StrategyState.Normal:
                strategyState = StrategyState.Merging;
                Character target1 = new Character();
                target1.maxHp = Random.Range(1, 8);
                target1.nowHp = target1.maxHp; // ���݂̗̑�
                target1.power = Random.Range(1, 8); // �U����
                target1.attackSpd = Random.Range(1, 8) + 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
                target1.skillType = Random.Range(1, 8); // �X�L���̎��
                target1.skillPoint = Random.Range(1, 8); // �X�L���|�C���g�A�O�`�P�U�ŕ\��
                target1.skillLevel = target1.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
                target1.rarerity = Random.Range(1, 3);
                Character target2 = new Character();
                target2.maxHp = Random.Range(1, 8);
                target2.nowHp = target2.maxHp; // ���݂̗̑�
                target2.power = Random.Range(1, 8); // �U����
                target2.attackSpd = Random.Range(1, 8) + 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
                target2.skillType = Random.Range(1, 8); // �X�L���̎��
                target2.skillPoint = Random.Range(1, 8); // �X�L���|�C���g�A�O�`�P�U�ŕ\��
                target2.skillLevel = target2.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
                target2.rarerity = Random.Range(1, 3);
                MergeManager.Instance.Activate(target1, target2);
                //�ʏ���
                break;
            case StrategyState.Having:
                //�L�����N�^�[�������������
                break;
            case StrategyState.Merging:
                MergeManager.Instance.PushAButton();
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
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
                break;
            case StrategyState.Having:
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
                BattleStageDisplayManager.Instance.UpdateCursol(cursol + 21);
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
                BattleStageDisplayManager.Instance.UpdateCursol(cursol + 21);
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
                BattleStageDisplayManager.Instance.UpdateCursol(cursol + 21);
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
                BattleStageDisplayManager.Instance.UpdateCursol(cursol + 21);
                break;
            case StrategyState.Merging:
                //�}�[�W���̉��
                break;
            case StrategyState.Recruiting:
                //�̗p���̉��
                break;
        }

    }
}
