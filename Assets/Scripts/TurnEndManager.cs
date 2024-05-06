using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndManager : MonoBehaviour
{
    public static TurnEndManager Instance;
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
    public void TurnEnd()
    {
        AllyArriveEnemyCampCheck();
        EnemyArriveAllyCampCheck();
        GameManager.Instance.AddTurn();
        BattleInformationDisplayManager.Instance.RefreshTurnsDisplay(GameManager.Instance.turns);
        BattleInformationDisplayManager.Instance.RefreshClearConditionText(GameManager.Instance.stageType, GameManager.Instance.goalPeople, GameManager.Instance.turns, GameManager.Instance.clearPeople, GameManager.Instance.clearTurn);

        //復活ターン更新
        for (int campIndex = 0; campIndex < 12; campIndex++)
        {
            Character ally = BattleStageManager.Instance.camp[campIndex];
            if (ally.charaState == Character.CharaState.Death)
            {
                ally.reviveTurn--;
                if (ally.reviveTurn <= 0)
                {
                    ally.Revive();
                }
                BattleStageDisplayManager.Instance.RefreshCharacter(ally);
            }
        }
    }
    public void EnemyArriveAllyCampCheck()
    {
        for (int lane = 1; lane <= BattleStageManager.Instance.laneCount; lane++)
        {
            int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, 1);
            Character character = BattleStageManager.Instance.frontline[frontlineIndex];
            if (character.exists && character.charaState == Character.CharaState.Enemy)
            {
                GameManager.Instance.GameOver();
                break;
            }
        }
    }
    public void AllyArriveEnemyCampCheck()
    {
        for (int lane = 1; lane <= BattleStageManager.Instance.laneCount; lane++)
        {
            int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, BattleStageManager.Instance.laneLength + 2);
            Character character = BattleStageManager.Instance.frontline[frontlineIndex];
            if (character.exists && character.charaState == Character.CharaState.Frontline)
            {
                GameManager.Instance.Goal();
                int waitingCampIndex = BattleStageManager.Instance.GetCampIndexByEncampment(character.encampment);
                Character waitingChara = BattleStageManager.Instance.camp[waitingCampIndex];
                waitingChara.charaState = Character.CharaState.Goal;
                BattleStageDisplayManager.Instance.RefreshCharacter(waitingChara);
                BattleStageManager.Instance.RemoveCharacter(-1,frontlineIndex);
            }
        }
    }
    public void ReduceCampReviveTurnAndRevive()
    {

    }
}
