using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int mass; // ���݂̃}�X�B0 �����w�A 1 ���� GameManager.Instance.LaneLength �܂ł����A GameManager.Instance.LaneLength+1 �}�X�ڂ��G�w
    public int maxHp; // �ő�̗�
    public int nowHp; // ���݂̗̑�
    public int power; // �U����
    public int attackSpd; // �U�����x�A���ۂ̐��l�ł͂Ȃ�4�{�������̂ŊǗ��A�U�����x�� 1.25 �ł���� 5 �Ƃ��ĊǗ�����
    public int skillType; // �X�L���̎��
    public int skillTurn; // �X�L���̃����[�h
    public int skillnowTurn; // �X�L���̎c��^�[�����B
    public int skillPoint; // �X�L���|�C���g�A�O�`�P�U�ŕ\��
    public int skillLevel; // �X�L�����x���A�X�L���|�C���g���R�Ŋ�������(�؂�̂�)�A�O�`�T�ŕ\��
    public int rarerity; // ���A���e�B�A���P�`�T�ŕ\��
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
