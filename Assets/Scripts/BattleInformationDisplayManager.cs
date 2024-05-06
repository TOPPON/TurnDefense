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
    [SerializeField] public TextMeshProUGUI PlayerStatusRarityText;
    [SerializeField] public TextMeshProUGUI PlayerStatusHPText;
    [SerializeField] public TextMeshProUGUI PlayerStatusAtkText;
    [SerializeField] public TextMeshProUGUI PlayerStatusAtkSpdText;
    [SerializeField] public TextMeshProUGUI PlayerStatusSkillTypeText;
    [SerializeField] public TextMeshProUGUI PlayerStatusSkillLevelText;
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
        turnsText.text = "�^�[�� " + turns.ToString()+"\n"+"(��Փx "+GameManager.Instance.difficulty+" )";
    }
    public void RefreshClearConditionText(int stageType, int people, int turns, int clearpeople, int clearturn)
    {
        string temptext = "�N���A�����F";
        switch (stageType)
        {
            case 0: //�^�[����+�l��
                temptext += clearturn.ToString() + "�^�[���ȓ���" + clearpeople.ToString() + "�l���G�w�ɂ�";
                break;
            case 1: //�^�[����
                temptext += clearturn.ToString() + "�^�[���h�q����";
                break;
        }
        temptext += "\n���݁@�@�@�F";
        switch (stageType)
        {
            case 0: //�^�[����+�l��
                temptext += turns.ToString() + "�^�[��(�c��" + (clearturn - turns).ToString() + "�^�[��) & " + people.ToString() + "�l(�c��" + (clearpeople - people).ToString() + "�l)";
                break;
            case 1: //�^�[����
                temptext += clearturn.ToString() + "�^�[��(�c��" + (clearturn - turns).ToString() + "�^�[��)";
                break;
        }
        clearConditionText.text = temptext;
    }
    public void RefreshMoneyText(int money)
    {
        MoneyText.text = "������\n$" + money.ToString();
    }
    public void RefreshSellPriceText(int sellPrice)
    {
        SellText.text = "���p\n$" + sellPrice.ToString();
    }
    public void RefreshCharacterStatusDisplay(Character target = null, bool forseNoOutput = false)
    {
        if (!forseNoOutput)
        {
            if (target.exists)
            {
                PlayerStatusRarityText.text = "�� " + target.rarity.ToString();
                PlayerStatusHPText.text = "�̗� " + target.nowHp.ToString() + "/" + target.maxHp.ToString();
                PlayerStatusAtkText.text = "�U�� " + target.power.ToString();
                string tempstring = ((int)(target.attackSpd / 4)).ToString();
                switch (target.attackSpd % 4)
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
                PlayerStatusAtkSpdText.text = "�U�� " + tempstring;
                PlayerStatusSkillTypeText.text = "�X�L����� " + target.skillType.ToString();
                PlayerStatusSkillLevelText.text = "�X�L�����x�� lv" + target.skillLevel.ToString();
                return;
            }
        }
        PlayerStatusRarityText.text = "";
        PlayerStatusHPText.text = "";
        PlayerStatusAtkText.text = "";
        PlayerStatusAtkSpdText.text = "";
        PlayerStatusSkillTypeText.text = "";
        PlayerStatusSkillLevelText.text = "";
    }
    public void RefreshCharacterMergeDisplay(Character target1, Character target2)
    {

    }
}
