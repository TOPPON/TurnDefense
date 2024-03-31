using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum TurnState
    {
        BeforeStrategy,
        Strategy,
        AfterStrategy,
        BeforeMarch,
        March,
        AfterMarch
    }
    public TurnState turnState;
    public int turns=0;
    public int goalPeople=0;
    public int stageType = 0;//0: 人数, 1: ターン
    public int clearPeople = 3;
    public int clearTurn = 100;//時間制限または防衛ターン数
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
        switch (turnState)
        {
            case TurnState.BeforeStrategy:
                turnState = TurnState.Strategy;
                break;
            case TurnState.Strategy:
                break;
            case TurnState.AfterStrategy:
                turnState = TurnState.BeforeMarch;
                break;
            case TurnState.BeforeMarch:
                // 各キャラが行動を決める
                turnState = TurnState.March;
                break;
            case TurnState.March:
                // 実際に行動
                turnState = TurnState.AfterMarch;
                break;
            case TurnState.AfterMarch:
                // クリア条件などを計算

                // ターン加算処理
                turnState = TurnState.BeforeStrategy;
                turns++;
                BattleInformationDisplayManager.Instance.RefreshTurnsDisplay(turns);
                BattleInformationDisplayManager.Instance.RefreshClearConditionText(stageType, goalPeople, turns, clearPeople, clearTurn);
                break;
        }
    }
    public void PushGOButton()
    {
        if (turnState == TurnState.Strategy)
        {
            //
            turnState = TurnState.AfterStrategy;
        }
    }
}
