using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    public static AttackAnimationManager Instance;

    //���̃Q�[����spd=4 (1.00) �̂Ƃ���5�b�ԂŌ܉�U���ł���悤�ɂ������B
    //20/spd ��U�����ł���d�g��
    //200�Ń}�b�N�X�ɂȂ�悤�ɂ��Ă��C������
    public enum AttackAnimationState
    {
        Before,
        Attacking,
        After
    }
    AttackAnimationState attackAnimationState;

    const float UPDATE_TIME = 0.05f;//��R�}�����b�ȏ�P�ʂŋ��߂邩
    float updateTimer = 0;

    int gageTime;//100�Ń}�b�N�X

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
                        //�ӂ���Ƃ��|��Ă��痼����������
                        //�Е������|��Ă�����|��Ă��Ȃ������ق�����}�X�i�߂�
                        //�������|��Ă����畜���]�[���ɏ���
                        //�G���|��Ă����炨���𗎂Ƃ�
                        //
                        if (allyCharacter.nowHp <= 0 && enemyCharacter.nowHp <= 0)
                        {
                            //�����Ɉړ�������
                            BattleStageManager.Instance.DieFrontlineCharacter(allyCharacter);
                            //�����𑝂₵�ēG������
                            int money = enemyCharacter.rarity * 5 + 5;// Todo:���炦��l�i�͗v����
                            BattleStageDisplayManager.Instance.OccurMoneyByDefeat(enemyCharacter.lane, enemyCharacter.mass, money);
                            BattleStageManager.Instance.DieFrontlineCharacter(enemyCharacter);
                        }
                        else if (allyCharacter.nowHp <= 0)
                        {
                            //�����Ɉړ�������
                            BattleStageManager.Instance.DieFrontlineCharacter(allyCharacter);
                            //�G����}�X�i�߂�
                            BattleStageManager.Instance.FrontlineCharacterMove(enemyCharacter, 0, -1);
                            //�G�̕\�����X�V����
                            //BattleStageDisplayManager.Instance.RefreshCharacter(enemyCharacter);
                        }
                        else if (enemyCharacter.nowHp <= 0)
                        {
                            //�����𑝂₵�ăL����������
                            //GameManager.Instance.GetMoney(enemyCharacter.rarity * 5 + 5);
                            int money = enemyCharacter.rarity * 5 + 5;// Todo:���炦��l�i�͗v����
                            BattleStageDisplayManager.Instance.OccurMoneyByDefeat(enemyCharacter.lane,enemyCharacter.mass,money);
                            BattleStageManager.Instance.DieFrontlineCharacter(enemyCharacter);
                            //��������}�X�i�߂�
                            BattleStageManager.Instance.FrontlineCharacterMove(allyCharacter);
                            //�����̕\�����X�V����
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
