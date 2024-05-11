using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int turns = 1;//;
    public int goalPeople;// = 0;
    public int stageType;// = 0; //0: 人数, 1: ターン
    public int clearPeople;// = 3; //クリアに必要な人数
    public int clearTurn;// = 100; //時間制限または防衛ターン数
    public int money = 0;
    public int difficulty = 1;
    public int enemyDefeatMoneyBase = 5;

    bool gameClear = false;
    bool gameOver = false;
    bool gameStart = true;
    float gameControlTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Invoke("Activate", 0.1f);
            SetMoney(50);
        }
        else Destroy(gameObject);
    }
    public void Activate()
    {
        BattleStageManager.Instance.Activate();
        if (SceneMoveManager.Instance != null)
        {
            difficulty = SceneMoveManager.Instance.difficulty;
        }
        BattleInformationDisplayManager.Instance.RefreshTurnsDisplay(GameManager.Instance.turns);
        BattleInformationDisplayManager.Instance.RefreshClearConditionText(GameManager.Instance.stageType, GameManager.Instance.goalPeople, GameManager.Instance.turns, GameManager.Instance.clearPeople, GameManager.Instance.clearTurn);
        clearPeople = difficulty + 2;
    }


    // Update is called once per frame
    void Update()
    {
        if (gameStart || gameClear || gameOver)
        {
            gameControlTimer -= Time.deltaTime;
            if (gameControlTimer <= 0)
            {
                if (gameClear)
                {
                    if (SceneMoveManager.Instance != null)
                    {
                        SceneMoveManager.Instance.MoveToGameClearScene();
                    }
                    //SceneManager.LoadScene("GameClearScene");
                }
                else if (gameOver)
                {
                    if (SceneMoveManager.Instance != null)
                    {
                        SceneMoveManager.Instance.MoveToGameOverScene();
                    }
                    //SceneManager.LoadScene("GameOverScene");}
                }
                if (gameStart) gameStart = false;
            }
        }
        else
        {
            switch (turnState)
            {
                case TurnState.BeforeStrategy:
                    //敵の発生
                    EnemyOccurManager.Instance.EnemyOccur();
                    turnState = TurnState.Strategy;
                    break;
                case TurnState.Strategy:
                    StrategyManager.Instance.UpdateStrategy();
                    break;
                case TurnState.AfterStrategy:
                    turnState = TurnState.BeforeMarch;
                    MarchManager.Instance.StartMarchPlan();
                    break;
                case TurnState.BeforeMarch:
                    // 各キャラが行動を決める
                    //turnState = TurnState.March;
                    MarchManager.Instance.UpdateMarchPlan();
                    // メモ：Character に次の行動それぞれメモしてもいいかも。Character.Action Character.nextAction=Wait,Battle,Skill,
                    // 横に移動するキャラはどうしようね
                    break;
                case TurnState.March:
                    // 実際に行動
                    MarchManager.Instance.UpdateMarch();
                    //turnState = TurnState.AfterMarch;
                    break;
                case TurnState.AfterMarch:
                    // クリア条件などを計算
                    TurnEndManager.Instance.TurnEnd();
                    // ターン加算処理
                    turnState = TurnState.BeforeStrategy;

                    break;
                case TurnState.Pause:
                    break;
                    //ポーズ機能
            }
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

    //金銭操作
    public void AddMoney(int value)
    {
        if (value <= 0)
        {
            print("error! invalid Money value:" + value);
        }
        money += value;
        BattleInformationDisplayManager.Instance.RefreshMoneyText(money);
    }
    public void ReduceMoney(int value)
    {
        if (value <= 0 || money < value)
        {
            print("error! invalid Money value:" + value);
        }
        money -= value;
        BattleInformationDisplayManager.Instance.RefreshMoneyText(money);
    }
    public void SetMoney(int value)
    {
        if (value < 0)
        {
            print("error! invalid Money value:" + value);
        }
        money = value;
        BattleInformationDisplayManager.Instance.RefreshMoneyText(money);
    }

    public void Goal()
    {
        goalPeople++;
        if (stageType == 0 && goalPeople >= clearPeople)
        {
            GameClear();
        }
        if (stageType == 0 && turns >= clearTurn)
        {
            GameOver();
        }
    }
    public void AddTurn()
    {
        turns++;
        if (stageType == 1 && turns >= clearTurn)
        {
            GameClear();
        }
    }
    public void GameOver()
    {
        print("GameOver!!");
        gameOver = true;
        gameControlTimer = 5f;
    }
    public void GameClear()
    {
        print("GameClear!!");
        gameClear = true;
        gameControlTimer = 5f;
    }

    public void CompleteMarchPlan()
    {
        if (turnState == TurnState.BeforeMarch)
        {
            turnState = TurnState.March;
            MarchManager.Instance.StartDoMarch();
        }
        else
        {
            print("error! invalid turnState:" + turnState);
        }
    }
    public void CompleteDoMarch()
    {
        if (turnState == TurnState.March)
        {
            turnState = TurnState.AfterMarch;
        }
        else
        {
            print("error! invalid turnState:" + turnState);
        }
    }

    //入力系
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
