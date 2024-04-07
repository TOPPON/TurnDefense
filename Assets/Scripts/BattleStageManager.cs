using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageManager : MonoBehaviour
{
    public static BattleStageManager Instance;
    public List<Character> camp = new List<Character>(); //�L�����v
    public List<Character> frontline = new List<Character>(); //���
    public int laneCount;
    public int laneLength;
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
    public void Activate()
    {
        laneCount = Random.Range(3, 15);
        laneLength = Random.Range(10, 20);
        BattleStageDisplayManager.Instance.SetLaneAndMass(laneCount, laneLength);
        for (int i = 0; i < laneCount; i++)
        {
            for (int j = 0; j < (laneLength + 2); j++)
            {
                Character temp = new Character();
                temp.mass = j + 1;
                temp.lane = i + 1;
                temp.encampment = 0;
                temp.SetExists(false);
                frontline.Add(temp);
            }
        }
        for (int i = 0; i < 18; i++)
        {
            Character temp = new Character();
            temp.mass = 0;
            temp.lane = 0;
            temp.encampment = i + 1;
            temp.SetExists(false);
            camp.Add(temp);
        }
    }
    //�L�������w�肵���J�[�\���ɒǉ�����B�󂫂��Ȃ������ꍇ�� false ��Ԃ�
    public bool AddCharacterToCamp(Character newCharacter, int cursol)
    {
        if (cursol < 0)
        {
            return false;
        }
        if (camp[cursol].exists == true)
        {
            return false;
        }
        camp[cursol].SetStatus(newCharacter, true);
        camp[cursol].SetExists(true);
        return true;
    }

    //�L������ǉ�����B�󂫂��Ȃ������ꍇ�� false ��Ԃ�
    public bool AddCharacter(Character newCharacter)
    {
        //���݂��Ȃ��L�����N�^�[�������ۂɃX�e�[�^�X�����ւ���
        foreach (Character chara in camp)
        {
            if (chara.exists == false)
            {
                chara.SetStatus(newCharacter, true);
                chara.SetExists(true);
                BattleStageDisplayManager.Instance.RefreshCharacter(chara);
                return true; //�ǉ�����
            }
        }
        return false; //�ǉ����s
    }
    public bool RemoveCharacter(int campIndex, int frontlineIndex)
    {
        if ((campIndex < 0 & frontlineIndex < 0) || (campIndex >= 0 & frontlineIndex >= 0))
        {
            //�������L���ȍۂ͍폜�ł��Ȃ�
            return false;
        }
        if (campIndex >= 0)
        {
            //�o���f�[�V����
            if (!camp[campIndex].exists) return false;
            camp[campIndex].SetExists(false);
            BattleStageDisplayManager.Instance.RefreshCharacter(camp[campIndex]);
            return true;
        }
        else if (frontlineIndex >= 0)
        {
            //�o���f�[�V����
            if (!frontline[frontlineIndex].exists) return false;
            frontline[frontlineIndex].SetExists(false);
            BattleStageDisplayManager.Instance.RefreshCharacter(frontline[frontlineIndex]);
            return true;
        }
        else return false;
    }
    public void MoveCharacter(Character newCharacter, int oldCursol, int newCursol)
    {

    }
    public int GetFrontlineIndex(int lane, int mass)
    {
        return (lane - 1) * (laneLength + 2) + mass - 1;
    }
    public int GetCampIndex(int index)
    {
        return index - 1;
    }
    public int GetCursolByCampIndex(int campIndex)
    {
        return -campIndex - 1;
    }
    public int GetCursolByFrontlineIndex(int frontlineIndex)
    {
        return frontlineIndex + 1;
    }
    public int GetCursolByFrontlineLaneAndMass(int frontlineLane, int frontlineMass)
    {
        return (frontlineLane - 1) * (laneLength + 2) + frontlineMass;
    }
}
