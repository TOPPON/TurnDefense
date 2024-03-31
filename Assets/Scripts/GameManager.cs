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
    public int stageType = 0;//0: �l��, 1: �^�[��
    public int clearPeople = 3;
    public int clearTurn = 100;//���Ԑ����܂��͖h�q�^�[����
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
                // �e�L�������s�������߂�
                turnState = TurnState.March;
                break;
            case TurnState.March:
                // ���ۂɍs��
                turnState = TurnState.AfterMarch;
                break;
            case TurnState.AfterMarch:
                // �N���A�����Ȃǂ��v�Z

                // �^�[�����Z����
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
