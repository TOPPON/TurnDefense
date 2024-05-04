using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackAnimationDisplayManager : MonoBehaviour
{
    public static AttackAnimationDisplayManager Instance;
    [SerializeField] GameObject attackAnimationDisplay;
    [SerializeField] Image TimeGage;

    [SerializeField] Image AllyCharacter;
    [SerializeField] Image AllyHpBar;
    [SerializeField] TextMeshProUGUI AllyHpBarText;
    [SerializeField] Image AllySpdBar;
    [SerializeField] TextMeshProUGUI AllyPowerText;
    [SerializeField] TextMeshProUGUI AllySpdText;
    [SerializeField] TextMeshProUGUI AllySkillText;

    [SerializeField] Image EnemyCharacter;
    [SerializeField] Image EnemyHpBar;
    [SerializeField] TextMeshProUGUI EnemyHpBarText;
    [SerializeField] Image EnemySpdBar;
    [SerializeField] TextMeshProUGUI EnemyPowerText;
    [SerializeField] TextMeshProUGUI EnemySpdText;
    [SerializeField] TextMeshProUGUI EnemySkillText;

    bool allyAttacking;
    bool enemyAttacking;
    float allyAttackingTimer = 0;
    float enemyAttackingTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void ActivateDisplay()
    {
        TimeGage.fillAmount = 0;
        attackAnimationDisplay.SetActive(true);
    }
    public void DeactivateDisplay()
    {
        attackAnimationDisplay.SetActive(false);
    }
    public void SetFirstAllyStatus(int skillType, int power, int spd, int nowHp, int maxHp)
    {
        AllyCharacter.sprite = DisplayCharacter.GetDisplayCharacterByType(skillType);
        AllyPowerText.text = power.ToString();
        AllySpdText.text = GetSpdText(spd);
        AllyHpBar.fillAmount = 1.0f * nowHp / maxHp;
        AllyHpBarText.text = nowHp.ToString() + " / " + maxHp.ToString();
    }
    public void SetFirstEnemyStatus(int skillType, int power, int spd, int nowHp, int maxHp)
    {
        EnemyCharacter.sprite = DisplayCharacter.GetDisplayCharacterByType(skillType);
        EnemyPowerText.text = power.ToString();
        EnemySpdText.text = GetSpdText(spd);
        EnemyHpBar.fillAmount = 1.0f * nowHp / maxHp;
        EnemyHpBarText.text = nowHp.ToString() + " / " + maxHp.ToString();
    }
    public void UpdateAllyHp(int nowHp, int maxHp)
    {
        AllyHpBar.fillAmount = 1.0f * nowHp / maxHp;
        AllyHpBarText.text = nowHp.ToString() + " / " + maxHp.ToString();
    }
    public void UpdateEnemyHp(int nowHp, int maxHp)
    {
        EnemyHpBar.fillAmount = 1.0f * nowHp / maxHp;
        EnemyHpBarText.text = nowHp.ToString() + " / " + maxHp.ToString();
    }
    public void UpdateTimeGage(float nowValue)
    {
        TimeGage.fillAmount = nowValue;
    }
    public void UpdateAllySpdBar(float nowValue)
    {
        AllySpdBar.fillAmount = nowValue;
    }
    public void UpdateEnemySpdBar(float nowValue)
    {
        EnemySpdBar.fillAmount = nowValue;
    }
    public void AllyAttack()
    {
        allyAttacking = true;
        allyAttackingTimer = 0.25f;
    }
    public void EnemyAttack()
    {
        enemyAttacking = true;
        enemyAttackingTimer = 0.25f;
    }
    // Update is called once per frame
    void Update()
    {
        if (allyAttacking)
        {
            allyAttackingTimer -= Time.deltaTime;
            AllyCharacter.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-250 + allyAttackingTimer * 200, 50);
            if (allyAttackingTimer < 0)
            {
                allyAttacking = false;
                AllyCharacter.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-250, 50);
            }
        } 
        if (enemyAttacking)
        {
            enemyAttackingTimer -= Time.deltaTime;
            EnemyCharacter.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(250 - enemyAttackingTimer * 200, 50);
            if (enemyAttackingTimer < 0)
            {
                enemyAttacking = false;
                EnemyCharacter.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(250, 50);
            }
        }
    }
    public string GetSpdText(int spd)
    {
        string tempstring = ((int)(spd / 4)).ToString();
        switch (spd % 4)
        {
            case 0:
                tempstring += ".00";
                break;
            case 1:
                tempstring += ".25";
                break;
            case 2:
                tempstring += ".50";
                break;
            case 3:
                tempstring += ".75";
                break;
        }
        return tempstring;
    }
}
