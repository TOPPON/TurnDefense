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
    public void AllyArriveEnemyCampCheck()
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
    public void EnemyArriveAllyCampCheck()
    {
        for (int lane = 1; lane <= BattleStageManager.Instance.laneCount; lane++)
        {
            int frontlineIndex = BattleStageManager.Instance.GetFrontlineIndexByLaneAndMass(lane, BattleStageManager.Instance.laneLength + 2);
            Character character = BattleStageManager.Instance.frontline[frontlineIndex];
            if (character.exists && character.charaState == Character.CharaState.Frontline)
            {
                GameManager.Instance.Goal();
            }
        }
    }
    public void ReduceCampReviveTurnAndRevive()
    {

    }
}
