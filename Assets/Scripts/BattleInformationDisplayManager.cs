using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleInformationDisplayManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI turnsText;
    [SerializeField] public TextMeshProUGUI clearConditionText;
    [SerializeField] public TextMeshProUGUI SellText;
    [SerializeField] public TextMeshProUGUI MoneyText;
    public static BattleInformationDisplayManager Instance;
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
    public void RefreshTurnsDisplay(int turns)
    {
        turnsText.text = "ターン " + turns.ToString(); 
    }
    public void RefreshClearConditionText(int stageType,int people,int turns,int clearpeople,int clearturn)
    {
        string temptext = "クリア条件：";
        switch (stageType)
        {
            case 0: //ターン数+人数
                temptext+=clearturn.ToString()+"ターン以内に"+clearpeople.ToString()+"人が敵陣につく";
                break;
            case 1: //ターン数
                temptext += clearturn.ToString() + "ターン防衛する";
                break;
        }
        temptext += "\n現在　　　：";
        switch (stageType)
        {
            case 0: //ターン数+人数
                temptext += turns.ToString() + "ターン(残り" + (clearturn-turns).ToString()+"ターン) & "+people.ToString()+"人(残り"+(clearpeople-people).ToString() + "人)";
                break;
            case 1: //ターン数
                temptext += clearturn.ToString() + "ターン(残り"+ (clearturn - turns).ToString() + "ターン)";
                break;
        }
        clearConditionText.text = temptext;
    }
    public void RefreshMoneyText(int money)
    {
        MoneyText.text = "所持金\n$"+money.ToString();
    }
    public void RefreshSellPriceText(int sellPrice)
    {
        SellText.text = "売却\n$"+ sellPrice.ToString();
    }
    public void RefreshCharacterStatusDisplay(Character target)
    {
        /*Target2Rarity.text = "星" + character.rarity.ToString();
        Target2HitPoint.text = "体力：" + character.maxHp.ToString();
        Target2Power.text = "力：" + character.power.ToString();
        // 攻速は管理している値とユーザに見せる値が異なるためユーザに見せる値を計算するロジックをつける
        string tempstring = ((int)(character.attackSpd / 4)).ToString();
        switch (character.attackSpd % 4)
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
        Target2AttackSpd.text = "攻速：" + tempstring;
        Target2Skill.text = "スキル：\n" + character.skillType.ToString() + "\nLv." + character.skillLevel.ToString() + "(" + character.skillPoint.ToString() + ")";*/
    }
    public void RefreshCharacterMergeDisplay(Character target1,Character target2)
    {

    }
}
