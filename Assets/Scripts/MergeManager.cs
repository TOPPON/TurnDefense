using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;
    int cursol;//0:���s 1:�߂�
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
        //�}�[�W���鏈��
        StrategyManager.Instance.FinishMerge();
        MergeDisplayManager.Instance.DeactivateDisplay();
        int skillResult = Random.Range(0, 2);
        switch (skillResult)
        {
            case 0:
                result.skillType = target1.skillType;//�^�[���̏����Ƃ��ǉ�����K�v�����肻��
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
        result.maxHp = target1.maxHp + target2.maxHp - 1; // �ő�̗�
        result.nowHp = result.maxHp; // ���݂̗̑�
        result.power = target1.power + target2.power - 1; // �U����
        result.attackSpd = target1.attackSpd + target2.attackSpd - 4; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
        //result.skillType; // �X�L���̎��
        //result.skillTurn; // �X�L���̃����[�h
        //result.skillnowTurn; // �X�L���̎c��^�[�����B
        result.skillPoint = target1.skillPoint + target2.skillPoint; // �X�L���|�C���g�A�O�`�P�U�ŕ\��
        result.skillLevel = result.skillPoint / 3; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
        result.rarity = target1.rarity + 1;
        result.reviveMaxTurn = result.rarity + 1;
        result.reviveTurn = 0;// Random.Range(0, result.reviveMaxTurn+1);//�f�o�b�O�p
        return result;
    }
}
