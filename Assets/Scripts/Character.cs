using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int mass; // ���݂̃}�X�B0 �����w�A 1 ���� GameManager.Instance.LaneLength �܂ł����A GameManager.Instance.LaneLength+1 �}�X�ڂ��G�w
    public int lane; // ���݂̗�B0 �����w�A������ 1,,,MaxLane�܂ŁB
    public int encampment; // ���w�̍��W�B0 ���Ȃ��A1 ���� 12 �܂ł� ally �A13 ���� 18 �܂ł��T��
    public int maxHp; // �ő�̗�
    public int nowHp; // ���݂̗̑�
    public int power; // �U����
    public int attackSpd; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
    public int skillType; // �X�L���̎��
    public int skillTurn; // �X�L���̃����[�h
    public int skillnowTurn; // �X�L���̎c��^�[�����B
    public int skillPoint; // �X�L���|�C���g�A�O�`�P�U�ŕ\��
    public int skillLevel; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
    public int rarity; // ���A���e�B�A���P�`�T�ŕ\��
    public int reviveTurn; //�����܂łɂ��Ɖ��^�[���K�v��
    public int reviveMaxTurn; //�����܂łɉ��^�[���K�v��
    public bool exists;//���݂��邩�ۂ�
    public enum CharaState
    {
        None,
        Death, //Ally����A����ŕ����X�A�z�u�]���┄�p�Ȃǂ͕s�\
        Ally, //Ally����A�o���O�A��������z�u�]�����ł���
        Reserve, //�T���A��������z�u�]�����ł���
        Waiting, //Ally����A�o����(mass,lane �ɂ͓������̂����L)
        Frontline, //���(encampment �ɑ҂�����L��)
        Goal,//����񂩂킩���
        Enemy
    }
    public CharaState charaState; //�L�����N�^�[�̏��
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetStatus(Character targetCharacter, bool keepPosition = false)
    {
        if (!keepPosition)
        {
            mass = targetCharacter.mass; // ���݂̃}�X�B0 �����w�A 1 ���� GameManager.Instance.LaneLength �܂ł����A GameManager.Instance.LaneLength+1 �}�X�ڂ��G�w
            lane = targetCharacter.lane; // ���݂̗�B0 �����w�A������ 1,,,MaxLane�܂ŁB
            encampment = targetCharacter.encampment; // ���w�̍��W�B0 ���Ȃ��A1 ���� 12 �܂ł� ally �A13 ���� 18 �܂ł��T��
        }
        maxHp = targetCharacter.maxHp; // �ő�̗�
        nowHp = targetCharacter.nowHp; // ���݂̗̑�
        power = targetCharacter.power; // �U����
        attackSpd = targetCharacter.attackSpd; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
        skillType = targetCharacter.skillType; // �X�L���̎��
        skillTurn = targetCharacter.skillTurn; // �X�L���̃����[�h
        skillnowTurn = targetCharacter.skillnowTurn; // �X�L���̎c��^�[�����B
        skillPoint = targetCharacter.skillPoint; // �X�L���|�C���g�A�O�`�P�U�ŕ\��
        skillLevel = targetCharacter.skillLevel; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
        rarity = targetCharacter.rarity; // ���A���e�B�A���P�`�T�ŕ\��
        exists = targetCharacter.exists;
        charaState = targetCharacter.charaState;
        reviveTurn = targetCharacter.reviveTurn; //�����܂łɂ��Ɖ��^�[���K�v��
        reviveMaxTurn = targetCharacter.reviveMaxTurn; //�����܂łɉ��^�[���K�v��
    }
    public void SetExists(bool exists)
    {
        this.exists = exists;
    }
    public void Revive()
    {
        if ((reviveTurn > 0) || (charaState == CharaState.Death))
        {
            print("Revive error!! invalid status reviveturn:" + reviveTurn + " Charastate:" + charaState);
            return;
        }
        reviveTurn = 0;
        UpdateCharacterStatusInCamp();
    }
    public void UpdateCharacterStatusInCamp()
    {
        if (encampment <= 0)
        {
            print("UpdateCharacterStatus error!! invalid encampment:" + encampment);
            return;
        }
        if (encampment <= 12)
        {
            charaState = CharaState.Ally;
        }
        else
        {
            charaState = CharaState.Reserve;
        }
    }
}
