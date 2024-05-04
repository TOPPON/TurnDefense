using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageManager : MonoBehaviour
{
    public static BattleStageManager Instance;
    public List<Character> camp = new List<Character>(); //キャンプ
    public List<Character> frontline = new List<Character>(); //戦線
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
        laneCount = 2;// Random.Range(3, 8);
        laneLength = 3;// Random.Range(5, 12);
        BattleStageDisplayManager.Instance.SetLaneAndMass(laneCount, laneLength);
        BattleStageDisplayManager.Instance.UpdateCursol(-1);
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
    public bool AddCharacterToFrontline(Character newCharacter, int lane, int mass)
    {
        int frontlineIndex = GetFrontlineIndexByLaneAndMass(lane, mass);
        print("frontlineIndex"+ frontlineIndex);
        if (frontline[frontlineIndex].exists == true)
        {
            return false;
        }
        frontline[frontlineIndex].SetStatus(newCharacter, true);
        frontline[frontlineIndex].SetExists(true);
        print(frontline[frontlineIndex].maxHp);
        BattleStageDisplayManager.Instance.RefreshCharacter(frontline[frontlineIndex]);
        return true;
    }
    //キャラを指定したカーソルに追加する。空きがなかった場合は false を返す
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
        BattleStageDisplayManager.Instance.RefreshCharacter(camp[cursol]);
        return true;
    }

    //キャンプにキャラを追加する。空きがなかった場合は false を返す
    public void AddCharacterToCamp(Character newCharacter)
    {
        //存在しないキャラクターがいた際にステータスを入れ替える
        foreach (Character chara in camp)
        {
            if (chara.exists == false)
            {
                chara.SetStatus(newCharacter, true);
                chara.SetExists(true);
                chara.UpdateCharacterStatusInCamp();
                BattleStageDisplayManager.Instance.RefreshCharacter(chara);
                return; //追加成功
            }
        }
        print("error AddCharacterToCamp"); //追加失敗
    }
    public bool CheckEnableAddCharacter()
    {
        foreach (Character chara in camp)
        {
            if (chara.exists == false)
            {
                return true;
            }
        }
        return false;
    }
    public bool RemoveCharacter(int campIndex, int frontlineIndex)
    {
        if ((campIndex < 0 & frontlineIndex < 0) || (campIndex >= 0 & frontlineIndex >= 0))
        {
            //両方が有効な際は削除できない
            return false;
        }
        if (campIndex >= 0)
        {
            //バリデーション
            if (!camp[campIndex].exists) return false;
            camp[campIndex].SetExists(false);
            camp[campIndex].mass = 0;
            camp[campIndex].lane = 0;
            BattleStageDisplayManager.Instance.RefreshCharacter(camp[campIndex]);
            return true;
        }
        else if (frontlineIndex >= 0)
        {
            //バリデーション
            if (!frontline[frontlineIndex].exists) return false;
            frontline[frontlineIndex].SetExists(false);
            frontline[frontlineIndex].encampment = 0;
            BattleStageDisplayManager.Instance.RefreshCharacter(frontline[frontlineIndex]);
            return true;
        }
        else return false;
    }
    public void MoveCharacter(Character newCharacter, int oldCursol, int newCursol)
    {

    }

    //陣地関連の変換
    public int GetCampIndexByCursol(int cursol)
    {
        return -cursol - 1;
    }
    public int GetCampIndexByEncampment(int encampment)
    {
        return encampment - 1;
    }
    public int GetCursolByCampIndex(int campIndex)
    {
        return -campIndex - 1;
    }
    public int GetCursolByEncampment(int encampment)
    {
        return -encampment;
    }
    public int GetEncampmentByCampIndex(int campIndex)
    {
        return campIndex + 1;
    }
    public int GetEncampmentByCursol(int cursol)
    {
        return -cursol;
    }

    //戦線関連の変換
    public Vector2 GetFrontlineLaneAndMassByCursol(int cursol)
    {
        return new Vector2((int)((cursol - 1) / (laneLength + 2)) + 1, (cursol - 1) % (laneLength + 2) + 1);
    }
    public Vector2 GetFrontlineLaneAndMassByFrontlineIndex(int frontlineIndex)
    {
        return new Vector2((int)(frontlineIndex / (laneLength + 2)) + 1, frontlineIndex % (laneLength + 2) + 1);
    }
    public int GetFrontlineIndexByCursol(int cursol)
    {
        return cursol - 1;
    }
    public int GetFrontlineIndexByLaneAndMass(int lane, int mass)
    {
        return (lane - 1) * (laneLength + 2) + mass - 1;
    }
    public int GetCursolByFrontlineIndex(int frontlineIndex)
    {
        return frontlineIndex + 1;
    }
    public int GetCursolByFrontlineLaneAndMass(int frontlineLane, int frontlineMass)
    {
        return (frontlineLane - 1) * (laneLength + 2) + frontlineMass;
    }
    //
    public void FrontlineCharacterMove(Character target, int xVec = 0, int yVec = 1)
    {
        int lane = target.lane;
        int mass = target.mass;
        int encampment = target.encampment;
        //print("baseEncampment:" + encampment);

        int frontlineIndex = GetFrontlineIndexByLaneAndMass(lane, mass);
        Character frontlineCharacter = frontline[frontlineIndex];
        int aheadLane = lane + xVec;
        int aheadMass = mass + yVec;
        if (aheadLane > laneCount)
        {
            print("laneが上限を超えた");
            aheadLane = laneCount;
        }
        if (aheadLane < 1)
        {
            print("laneが下限を下回った");
            aheadLane = 1;
        }
        if (aheadMass > laneLength + 2)
        {
            print("massが上限を超えた");
            aheadMass = laneLength + 2;
        }
        if (aheadMass < 1)
        {
            print("massが下限を下回った");
            aheadMass = 1;
        }
        AddCharacterToFrontline(frontlineCharacter, aheadLane, aheadMass);
        RemoveCharacter(-1, frontlineIndex);

        if (target.charaState != Character.CharaState.Enemy)
        {
            int newFrontlineIndex = GetFrontlineIndexByLaneAndMass(aheadLane, aheadMass);
            BattleStageManager.Instance.frontline[newFrontlineIndex].encampment = encampment;
            int campIndex = GetCampIndexByEncampment(encampment);
            Character waitingCharacter = camp[campIndex];
            waitingCharacter.lane = aheadLane;
            waitingCharacter.mass = aheadMass;
            //print("waiting.encampment:" + waitingCharacter.encampment);
        }
    }
    public void DieFrontlineCharacter(Character target)
    {
        int frontlineIndex = GetFrontlineIndexByLaneAndMass(target.lane, target.mass);
        //frontline のキャラを削除する
        if (target.charaState == Character.CharaState.Enemy)
        {
            RemoveCharacter(-1,frontlineIndex);
        }
        else
        {
            //味方
            int encampment = target.encampment;
            int campIndex = GetCampIndexByEncampment(encampment);
            Character waitingCharacter = camp[campIndex];
            waitingCharacter.reviveTurn = waitingCharacter.reviveMaxTurn;
            waitingCharacter.charaState = Character.CharaState.Death;
            waitingCharacter.lane = 0;
            waitingCharacter.mass = 0;
            RemoveCharacter(-1, frontlineIndex);
            BattleStageDisplayManager.Instance.RefreshCharacter(waitingCharacter);
        }
    }
}
