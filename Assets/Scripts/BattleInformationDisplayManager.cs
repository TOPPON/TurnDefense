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
        turnsText.text = "�^�[�� " + turns.ToString(); 
    }
    public void RefreshClearConditionText(int stageType,int people,int turns,int clearpeople,int clearturn)
    {
        string temptext = "�N���A�����F";
        switch (stageType)
        {
            case 0: //�^�[����+�l��
                temptext+=clearturn.ToString()+"�^�[���ȓ���"+clearpeople.ToString()+"�l���G�w�ɂ�";
                break;
            case 1: //�^�[����
                temptext += clearturn.ToString() + "�^�[���h�q����";
                break;
        }
        temptext += "\n���݁@�@�@�F";
        switch (stageType)
        {
            case 0: //�^�[����+�l��
                temptext += turns.ToString() + "�^�[��(�c��" + (clearturn-turns).ToString()+"�^�[��) & "+people.ToString()+"�l(�c��"+(clearpeople-people).ToString() + "�l)";
                break;
            case 1: //�^�[����
                temptext += clearturn.ToString() + "�^�[��(�c��"+ (clearturn - turns).ToString() + "�^�[��)";
                break;
        }
        clearConditionText.text = temptext;
    }
    public void RefreshMoneyText(int money)
    {
        MoneyText.text = "������\n$"+money.ToString();
    }
    public void RefreshSellPriceText(int sellPrice)
    {
        SellText.text = "���p\n$"+ sellPrice.ToString();
    }
    public void RefreshCharacterStatusDisplay(Character target)
    {
        /*Target2Rarity.text = "��" + character.rarity.ToString();
        Target2HitPoint.text = "�̗́F" + character.maxHp.ToString();
        Target2Power.text = "�́F" + character.power.ToString();
        // �U���͊Ǘ����Ă���l�ƃ��[�U�Ɍ�����l���قȂ邽�߃��[�U�Ɍ�����l���v�Z���郍�W�b�N������
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
        Target2AttackSpd.text = "�U���F" + tempstring;
        Target2Skill.text = "�X�L���F\n" + character.skillType.ToString() + "\nLv." + character.skillLevel.ToString() + "(" + character.skillPoint.ToString() + ")";*/
    }
    public void RefreshCharacterMergeDisplay(Character target1,Character target2)
    {

    }
}
