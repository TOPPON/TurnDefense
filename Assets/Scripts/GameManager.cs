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
        AfterMarch,
        Pause
    }
    public TurnState turnState;
    public TurnState beforeState; //一個前のステート
    public int turns=0;
    public int goalPeople=0;
    public int stageType = 0; //0: 人数, 1: ターン
    public int clearPeople = 3; //クリアに必要な人数
    public int clearTurn = 100; //時間制限または防衛ターン数

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Invoke("Activate",1f);
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
            case TurnState.Pause:
                break;
                //ポーズ機能
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
    public void Activate()
    {
        BattleStageManager.Instance.Activate();
    }
    public void PushAButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushAButton();
                break;
        }
    }
    public void PushBButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushBButton();
                break;
        }
    }
    public void PushLeftButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushLeftButton();
                break;
        }
    }
    public void PushRightButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushRightButton();
                break;
        }
    }
    public void PushUpButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushUpButton();
                break;
        }
    }
    public void PushDownButton()
    {
        switch (turnState)
        {
            case TurnState.Strategy:
                StrategyManager.Instance.PushDownButton();
                break;
        }
    }
    public void PushStartButton()
    {

    }
}
