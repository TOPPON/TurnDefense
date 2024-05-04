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
    public TurnState beforeState; //��O�̃X�e�[�g
    public int turns = 0;
    public int goalPeople = 0;
    public int stageType = 0; //0: �l��, 1: �^�[��
    public int clearPeople = 3; //�N���A�ɕK�v�Ȑl��
    public int clearTurn = 100; //���Ԑ����܂��͖h�q�^�[����
    public int money = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Invoke("Activate", 0.1f);
            SetMoney(1000);
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
                StrategyManager.Instance.UpdateStrategy();
                break;
            case TurnState.AfterStrategy:
                turnState = TurnState.BeforeMarch;
                MarchManager.Instance.StartMarchPlan();
                break;
            case TurnState.BeforeMarch:
                // �e�L�������s�������߂�
                //turnState = TurnState.March;
                MarchManager.Instance.UpdateMarchPlan();
                // �����FCharacter �Ɏ��̍s�����ꂼ�ꃁ�����Ă����������BCharacter.Action Character.nextAction=Wait,Battle,Skill,
                // ���Ɉړ�����L�����͂ǂ����悤��
                break;
            case TurnState.March:
                // ���ۂɍs��
                MarchManager.Instance.UpdateMarch();
                //turnState = TurnState.AfterMarch;
                break;
            case TurnState.AfterMarch:
                // �N���A�����Ȃǂ��v�Z

                // �^�[�����Z����
                turnState = TurnState.BeforeStrategy;
                turns++;
                BattleInformationDisplayManager.Instance.RefreshTurnsDisplay(turns);
                BattleInformationDisplayManager.Instance.RefreshClearConditionText(stageType, goalPeople, turns, clearPeople, clearTurn);
                break;
            case TurnState.Pause:
                break;
                //�|�[�Y�@�\
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

    //���K����
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
