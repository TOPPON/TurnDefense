using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOccurManager : MonoBehaviour
{
    public static EnemyOccurManager Instance;
    int k1;
    int k2;
    int k3;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            k1 = Random.Range(1, 10);
            k2 = Random.Range(1, 10);
            k3 = Random.Range(0, 3);
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyOccur()
    {
        //ターン数に応じて敵を発生させる。弱い敵から徐々に強くなっていく
        int turns = GameManager.Instance.turns;

        //ゲーム難易度。敵の強くなる速度、発生率が変わる。
        int difficulty = 2;

        //イメージ
        //dif1 t=1~9 strong=0(星0) 10~18 strong=1(星1) 19~27 strong=2(星2) n(10-dif)+1~(n+1)(10-dif) strong=n(星は0(0),1(1),2(2),3(4),4(8),5(10)の)強いやつ、発生率 70%
        //dif2 t=1~8 strong=0(星0) 9~16 strong=1(星1) 17~24 strong=2(星2) n(10-dif)+1~(n+1)(10-dif) strong=n(星は0(0),1(1),2(2),3(4),4(8),5(10)の)強いやつ、発生率 75%
        //dif3 t=1~7 strong=0(星0) 8~14 strong=1(星1) 15~21 strong=2(星2) n(10-dif)+1~(n+1)(10-dif) strong=n(星は0(0),1(1),2(2),3(4),4(8),5(10)の)強いやつ、発生率 80%

        //発生させるかどうかの判断
        if ((difficulty * 5 + 70) * turns / 100 == (difficulty * 5 + 70) * (turns + 1) / 100+2)//無条件で発生させるように変更している
        {
            return;
        }
        int occurs = 1;
        //if (Random.Range(0, 100) < difficulty * 5 + 10) occurs = 2;
        for (int o = 0; o < occurs; o++)
        {
            Character newEnemy = new Character();
            newEnemy.maxHp = 1;
            newEnemy.power = 1;
            newEnemy.attackSpd = 4;
            newEnemy.skillPoint = 0;
            int n = (int)(turns / (7.5f - difficulty));
            for (int i = 0; i < n; i++)//ランダムステータス割り振り
            {
                int index = Random.Range(1, 5);
                switch (index)
                {
                    case 1://hp
                        newEnemy.maxHp++;
                        break;
                    case 2://power
                        newEnemy.power++;
                        break;
                    case 3://spd
                        newEnemy.attackSpd++;
                        break;
                    case 4://skill
                        newEnemy.skillPoint++;
                        break;
                }
            }
            newEnemy.nowHp = newEnemy.maxHp;
            newEnemy.skillType = 12;//Random.Range(1, 3); // スキルの種類
            newEnemy.skillLevel = newEnemy.skillPoint / 3; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
            if (n == 0) newEnemy.rarity = 0;
            else if (n == 1) newEnemy.rarity = 1;
            else if (n >= 2 && n <= 3) newEnemy.rarity = 2;
            else if (n >= 4 && n <= 7) newEnemy.rarity = 3;
            else if (n >= 8 && n <= 15) newEnemy.rarity = 4;
            else if (n >= 16) newEnemy.rarity = 5;
            newEnemy.charaState = Character.CharaState.Enemy;
            newEnemy.lane = ((int)(turns * turns * (10 + k1) / 10.0f +  turns * (10 + k2) / 10.0f + k3))%BattleStageManager.Instance.laneCount+1;//Random.Range(1, BattleStageManager.Instance.laneCount + 1);
            newEnemy.mass = BattleStageManager.Instance.laneLength + 2;
            BattleStageManager.Instance.AddCharacterToFrontline(newEnemy, newEnemy.lane, newEnemy.mass);
        }
    }
}
