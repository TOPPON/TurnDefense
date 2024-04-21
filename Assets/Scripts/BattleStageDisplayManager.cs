using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStageDisplayManager : MonoBehaviour
{
    public static BattleStageDisplayManager Instance;
    [SerializeField] GameObject LaneObject;
    [SerializeField] GameObject MassObject;
    [SerializeField] GameObject FirstLineObject;
    [SerializeField] List<RectTransform> CursolPosition;
    [SerializeField] List<DisplayCharacter> CharaDisplayObject;//必要があれば追加する
    [SerializeField] GameObject CursolObject;
    [SerializeField] GameObject NormalCursolObject;
    [SerializeField] GameObject BattleStageObject;
    [SerializeField] GameObject DisplayCharacterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //SetLaneAndMass(5,8);
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLaneAndMass(int laneCount, int laneLength)
    {
        for (int i = 0; i < laneCount; i++)
        {
            GameObject newLane = Instantiate(LaneObject);
            newLane.transform.parent = FirstLineObject.transform;
            newLane.GetComponent<RectTransform>().localPosition = new Vector3(1440.0f / (laneCount + 1) * (i + 1) - 720, 0);
            RectTransform[] edgeMasses = newLane.GetComponentsInChildren<RectTransform>();
            RectTransform startTransform = new RectTransform();
            RectTransform goalTransform = new RectTransform();
            for (int j = 0; j < edgeMasses.Length; j++)
            {
                switch (edgeMasses[j].gameObject.name)
                {
                    case "StartMass":
                        startTransform = edgeMasses[j];
                        break;
                    case "GoalMass":
                        goalTransform = edgeMasses[j];
                        break;
                }
            }
            //スタートマスを登録
            CursolPosition.Add(startTransform);
            for (int j = 0; j < laneLength; j++)
            {
                GameObject newMass = Instantiate(MassObject);
                newMass.transform.parent = newLane.transform;
                newMass.GetComponent<RectTransform>().localPosition = new Vector3(0, 700.0f / (laneLength + 1) * (j + 1) - 350);
                CursolPosition.Add(newMass.GetComponent<RectTransform>());
            }
            //ゴールマスを登録
            CursolPosition.Add(goalTransform);
        }
    }
    public void UpdateCursol(int cursol)
    {
        CursolObject.GetComponent<RectTransform>().position = CursolPosition[cursol + 21].position;
    }
    public void ActivateNormalCursol(int cursol)
    {
        print(cursol);
        NormalCursolObject.SetActive(true);
        NormalCursolObject.GetComponent<RectTransform>().position = CursolPosition[cursol + 21].position;
    }
    public void DeactivateNormalCursol()
    {
        NormalCursolObject.SetActive(false);
    }
    public Vector3 GetCursolPosition(int cursol)
    {
        return CursolPosition[cursol + 21].position;
    }
    public void RefreshCharacter(Character character)
    {

        int cursol = 0;
        switch (character.charaState)
        {
            case Character.CharaState.None:
                print("Noneを消そうとしています");
                break;
            case Character.CharaState.Ally:
            case Character.CharaState.Reserve://味方の陣地にいる場合
            case Character.CharaState.Waiting://味方の陣地にいる場合
            case Character.CharaState.Death:
                print("encampment:" + character.encampment);
                cursol = BattleStageManager.Instance.GetCursolByEncampment(character.encampment);
                break;
            case Character.CharaState.Frontline:
            case Character.CharaState.Goal:
            case Character.CharaState.Enemy:
                cursol = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(character.lane, character.mass);
                break;
        }
        print("cursol:" + cursol);
        if (character.exists)
        {
            for (int i = CharaDisplayObject.Count - 1; i >= 0; i--)
            {
                DisplayCharacter dc = CharaDisplayObject[i];
                if (dc.cursol == cursol)
                {
                    //求めたい形式であるかチェック
                    if (dc.charaType == character.skillType && dc.displayCharaState == character.charaState)
                    {
                        //同じキャラクター
                        if (dc.displayCharaState == Character.CharaState.Death)
                        {
                            if (dc.reviveTurn != character.reviveTurn)
                            {
                                //復活ターンを更新
                                dc.reviveTurn = character.reviveTurn;
                            }
                            else
                            {

                            }
                        }
                    }
                    return;
                }
            }
            //スキルタイプと状態をあわせて新規作成
            DisplayCharacter newDisplayChara;
            //switch();
            newDisplayChara = Instantiate(DisplayCharacterPrefab).GetComponent<DisplayCharacter>();
            newDisplayChara.transform.parent = BattleStageObject.transform;
            newDisplayChara.SetAll(cursol, character.skillType, character.reviveTurn, character.reviveMaxTurn, character.rarity, character.charaState);
            CharaDisplayObject.Add(newDisplayChara);
        }
        else
        {
            print("CharaDisplayObjectCount:" + CharaDisplayObject.Count);
            for (int i = CharaDisplayObject.Count - 1; i >= 0; i--)
            {
                DisplayCharacter dc = CharaDisplayObject[i];
                if (dc.cursol == cursol)
                {
                    CharaDisplayObject.RemoveAt(i);
                    Destroy(dc.gameObject);
                }
            }
        }
    }
}
