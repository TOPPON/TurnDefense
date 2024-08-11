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
    [SerializeField] GameObject GoldCoinPrefab;
    [SerializeField] GameObject SilverCoinPrefab;
    [SerializeField] GameObject moneyDisplayObject;
    [SerializeField] GameObject BattleSymbolPrefab;

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
            newLane.transform.SetParent(FirstLineObject.transform, false);
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
                newMass.transform.SetParent(newLane.transform, false);
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
    public void RefreshCharacter(Character character, Vector2 moveVec = default(Vector2), bool isBirth = false, bool isDeath = false)
    {
        int cursol = 0;
        switch (character.charaState)
        {
            case Character.CharaState.None:
                //print("Noneを消そうとしています");
                break;
            case Character.CharaState.Ally:
            case Character.CharaState.Reserve://味方の陣地にいる場合
            case Character.CharaState.Waiting://味方の陣地にいる場合
            case Character.CharaState.Goal:
            case Character.CharaState.Death:
                //print("encampment:" + character.encampment);
                cursol = BattleStageManager.Instance.GetCursolByEncampment(character.encampment);
                break;
            case Character.CharaState.Frontline:
            case Character.CharaState.Enemy:
                cursol = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(character.lane, character.mass);
                break;
        }
        //print("cursol:" + cursol);
        if (character.exists)
        {
            for (int i = CharaDisplayObject.Count - 1; i >= 0; i--)
            {
                DisplayCharacter dc = CharaDisplayObject[i];
                if (dc.cursol == cursol)
                {
                    //求めたい形式であるかチェック
                    /*if (dc.charaType == character.skillType && dc.displayCharaState == character.charaState)
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
                    }*/
                    dc.SetAll(cursol, character.skillType, character.reviveTurn, character.reviveMaxTurn, character.rarity, character.charaState, character.nowHp, character.maxHp, character.power, character.attackSpd, moveVec, isBirth, isDeath);
                    return;
                }
            }
            //スキルタイプと状態をあわせて新規作成
            DisplayCharacter newDisplayChara;
            //switch();
            newDisplayChara = Instantiate(DisplayCharacterPrefab).GetComponent<DisplayCharacter>();
            newDisplayChara.transform.SetParent(BattleStageObject.transform, false);
            newDisplayChara.SetAll(cursol, character.skillType, character.reviveTurn, character.reviveMaxTurn, character.rarity, character.charaState, character.nowHp, character.maxHp, character.power, character.attackSpd, moveVec, isBirth, isDeath);
            CharaDisplayObject.Add(newDisplayChara);
        }
        else
        {
            //print("CharaDisplayObjectCount:" + CharaDisplayObject.Count);
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
    public void OccurMoneyByDefeat(int lane, int mass, int value)
    {
        print("OccurMoneyByDefeat" + lane + "&" + mass + "&" + value + "&");
        int cursol = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(lane, mass);
        Vector3 position = GetCursolPosition(cursol);
        while (value >= 5)
        {
            value -= 5;
            DisplayCoin displayCoin;
            displayCoin = Instantiate(GoldCoinPrefab).GetComponent<DisplayCoin>();
            displayCoin.transform.SetParent(BattleStageObject.transform, false);
            displayCoin.Occur(position, 5, moneyDisplayObject);
        }
        while (value >= 1)
        {
            value -= 1;
            DisplayCoin displayCoin;
            displayCoin = Instantiate(SilverCoinPrefab).GetComponent<DisplayCoin>();
            displayCoin.transform.SetParent(BattleStageObject.transform, false);
            displayCoin.Occur(position, 1, moneyDisplayObject);
        }
    }
    public void OccurBattleSymbol(int lane1, int mass1, int lane2, int mass2)
    {
        int cursol1 = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(lane1, mass1);
        int cursol2 = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(lane2, mass2);

        Vector3 position1 = GetCursolPosition(cursol1);
        Vector3 position2 = GetCursolPosition(cursol2);

        SymbolAnimation symbolAnimation;
        symbolAnimation = Instantiate(BattleSymbolPrefab).GetComponent<SymbolAnimation>();
        symbolAnimation.transform.SetParent(BattleStageObject.transform, false);
        symbolAnimation.Occur(SymbolAnimation.SymbolType.BattleStart, (position1 + position2) / 2, 1f);
    }
}
