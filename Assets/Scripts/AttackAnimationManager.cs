using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    public static AttackAnimationManager Instance;

    //このゲームはspd=4 (1.00) のときに5秒間で五回攻撃できるようにしたい。
    //20/spd 回攻撃ができる仕組み
    //200でマックスになるようにしてた気がする
    public enum AttackAnimationState
    {
        Before,
        Attacking,
        After
    }
    AttackAnimationState attackAnimationState;

    const float UPDATE_TIME = 0.05f;//一コマを何秒以上単位で求めるか
    float updateTimer = 0;

    int gageTime;//100でマックス

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
    public void Activate(Character ally, Character enemy)
    {
        AttackAnimationDisplayManager.Instance.ActivateDisplay();
        allyCharacter = ally;
        enemyCharacter = enemy;
        AttackAnimationDisplayManager.Instance.SetFirstAllyStatus(allyCharacter.skillType, allyCharacter.power, allyCharacter.attackSpd, allyCharacter.nowHp, allyCharacter.maxHp);
        AttackAnimationDisplayManager.Instance.SetFirstEnemyStatus(enemyCharacter.skillType, enemyCharacter.power, enemyCharacter.attackSpd, enemyCharacter.nowHp, enemyCharacter.maxHp);
        AttackAnimationDisplayManager.Instance.UpdateAllySpdBar(0);
        AttackAnimationDisplayManager.Instance.UpdateEnemySpdBar(0);
        gageTime = 0;
        allyAttackTimer = 0;
        enemyAttackTimer = 0;
        updateTimer = -1f;
        attackAnimationState = AttackAnimationState.Before;
    }
    public void Deactivate()
    {
        AttackAnimationDisplayManager.Instance.DeactivateDisplay();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateAttackAnimation()
    {
        switch (attackAnimationState)
        {
            case AttackAnimationState.Before:
                if (updateTimer > 1)
                {
                    attackAnimationState = AttackAnimationState.Attacking;
                    updateTimer = 0;
                }
                break;
            case AttackAnimationState.Attacking:
                if (updateTimer > UPDATE_TIME)
                {
                    gageTime++;
                    AttackAnimationDisplayManager.Instance.UpdateTimeGage(gageTime / 100.0f);
                    updateTimer = 0;

                    allyAttackTimer += allyCharacter.attackSpd;
                    enemyAttackTimer += enemyCharacter.attackSpd;

                    AttackAnimationDisplayManager.Instance.UpdateAllySpdBar(allyAttackTimer / 200.0f);
                    AttackAnimationDisplayManager.Instance.UpdateEnemySpdBar(enemyAttackTimer / 200.0f);
                    if (allyAttackTimer >= 200)
                    {
                        allyAttackTimer -= 200;
                        enemyCharacter.nowHp -= allyCharacter.power;
                        if (enemyCharacter.nowHp < 0) enemyCharacter.nowHp = 0;
                        AttackAnimationDisplayManager.Instance.UpdateEnemyHp(enemyCharacter.nowHp, enemyCharacter.maxHp);
                        AttackAnimationDisplayManager.Instance.AllyAttack();
                    }
                    if (enemyAttackTimer >= 200)
                    {
                        enemyAttackTimer -= 200;
                        allyCharacter.nowHp -= enemyCharacter.power;
                        if (allyCharacter.nowHp < 0) allyCharacter.nowHp = 0;
                        AttackAnimationDisplayManager.Instance.UpdateAllyHp(allyCharacter.nowHp, allyCharacter.maxHp);
                        AttackAnimationDisplayManager.Instance.EnemyAttack();
                    }

                    if (allyCharacter.nowHp <= 0 || enemyCharacter.nowHp <= 0)
                    {
                        //ふたりとも倒れてたら両方消すだけ
                        //片方だけ倒れていたら倒れていなかったほうを一マス進める
                        //味方が倒れていたら復活ゾーンに準備
                        //敵が倒れていたらお金を落とす
                        //
                        if (allyCharacter.nowHp <= 0 && enemyCharacter.nowHp <= 0)
                        {
                            //復活に移動させる
                            BattleStageManager.Instance.DieFrontlineCharacter(allyCharacter);
                            //お金を増やして敵を消す
                            int money = enemyCharacter.rarity * 5 + 5;// Todo:もらえる値段は要調整
                            BattleStageDisplayManager.Instance.OccurMoneyByDefeat(enemyCharacter.lane, enemyCharacter.mass, money);
                            BattleStageManager.Instance.DieFrontlineCharacter(enemyCharacter);
                        }
                        else if (allyCharacter.nowHp <= 0)
                        {
                            //復活に移動させる
                            BattleStageManager.Instance.DieFrontlineCharacter(allyCharacter);
                            //敵を一マス進める
                            BattleStageManager.Instance.FrontlineCharacterMove(enemyCharacter, 0, -1);
                            //敵の表示を更新する
                            //BattleStageDisplayManager.Instance.RefreshCharacter(enemyCharacter);
                        }
                        else if (enemyCharacter.nowHp <= 0)
                        {
                            //お金を増やしてキャラを消す
                            //GameManager.Instance.GetMoney(enemyCharacter.rarity * 5 + 5);
                            int money = enemyCharacter.rarity * 5 + 5;// Todo:もらえる値段は要調整
                            BattleStageDisplayManager.Instance.OccurMoneyByDefeat(enemyCharacter.lane,enemyCharacter.mass,money);
                            BattleStageManager.Instance.DieFrontlineCharacter(enemyCharacter);
                            //味方を一マス進める
                            BattleStageManager.Instance.FrontlineCharacterMove(allyCharacter);
                            //味方の表示を更新する
                            //BattleStageDisplayManager.Instance.RefreshCharacter(enemyCharacter);
                        }
                        //
                        attackAnimationState = AttackAnimationState.After;
                    }
                    if (gageTime >= 100)
                    {
                        attackAnimationState = AttackAnimationState.After;
                    }
                }
                break;
            case AttackAnimationState.After:
                if (updateTimer > 1)
                {
                    MarchManager.Instance.FinishAttackAnimation();
                }
                break;
        }
        updateTimer += Time.deltaTime;

    }
}
