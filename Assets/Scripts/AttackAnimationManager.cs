using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    public static AttackAnimationManager Instance;

    //このゲームはspd=4 (1.00) のときに5秒間で五回攻撃できるようにしたい。
    //20/spd 回攻撃ができる仕組み
    //200でマックスになるようにしてた気がする
    const float UPDATE_TIME = 0.02f;//一コマを何秒以上単位で求めるか
    float updateTimer = 0;

    int gageTime;//250でマックス

    int allyAttackTimer = 0;
    int enemyAttackTimer = 0;

    Character allyCharacter;
    Character enemyCharacter;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    void Activate(Character ally, Character enemy)
    {
        AttackAnimationDisplayManager.Instance.ActivateDisplay();
        allyCharacter = ally;
        enemyCharacter = enemy;
    }
    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer > UPDATE_TIME)
        {
            gageTime++;
            AttackAnimationDisplayManager.Instance.UpdateTimeGage(gageTime / 250.0f);
            updateTimer = 0;

            allyAttackTimer += allyCharacter.attackSpd;
            enemyAttackTimer += enemyCharacter.attackSpd;

            AttackAnimationDisplayManager.Instance.UpdateAllySpdBar(allyAttackTimer / 200.0f);
            AttackAnimationDisplayManager.Instance.UpdateEnemySpdBar(enemyAttackTimer / 200.0f);
            if (allyAttackTimer >= 200)
            {
                allyAttackTimer -= 200;
                enemyCharacter.nowHp -= allyCharacter.power;
                AttackAnimationDisplayManager.Instance.UpdateEnemyHp(enemyCharacter.nowHp, enemyCharacter.maxHp);
                AttackAnimationDisplayManager.Instance.AllyAttack();
            }
            if (enemyAttackTimer >= 200)
            {
                enemyAttackTimer -= 200;
                allyCharacter.nowHp -= enemyCharacter.power;
                AttackAnimationDisplayManager.Instance.UpdateEnemyHp(enemyCharacter.nowHp, enemyCharacter.maxHp);
                AttackAnimationDisplayManager.Instance.EnemyAttack();
            }

            if (allyCharacter.nowHp <= 0 || enemyCharacter.nowHp <= 0)
            {

            }
        }
    }
}
