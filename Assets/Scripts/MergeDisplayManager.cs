using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MergeDisplayManager : MonoBehaviour
{
    public static MergeDisplayManager Instance;
    // Start is called before the first frame update
    [SerializeField] GameObject mergeDisplay;
    [SerializeField] TextMeshProUGUI Target1Rarity;
    [SerializeField] TextMeshProUGUI Target1HitPoint;
    [SerializeField] TextMeshProUGUI Target1Power;
    [SerializeField] TextMeshProUGUI Target1AttackSpd;
    [SerializeField] TextMeshProUGUI Target1Skill;
    [SerializeField] TextMeshProUGUI Target2Rarity;
    [SerializeField] TextMeshProUGUI Target2HitPoint;
    [SerializeField] TextMeshProUGUI Target2Power;
    [SerializeField] TextMeshProUGUI Target2AttackSpd;
    [SerializeField] TextMeshProUGUI Target2Skill;
    [SerializeField] TextMeshProUGUI ResultRarity;
    [SerializeField] TextMeshProUGUI ResultHitPoint;
    [SerializeField] TextMeshProUGUI ResultPower;
    [SerializeField] TextMeshProUGUI ResultAttackSpd;
    [SerializeField] TextMeshProUGUI ResultSkill;
    [SerializeField] RectTransform[] CursolPosition;
    [SerializeField] RectTransform Cursol;
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
    public void SetTarget1Character(Character character)
    {
        Target1Rarity.text = "��" + character.rarity.ToString();
        Target1HitPoint.text = "�̗́F" + character.maxHp.ToString();
        Target1Power.text = "�́F" + character.power.ToString();
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
        Target1AttackSpd.text = "�U���F" + tempstring;
        Target1Skill.text = "�X�L���F\n" + character.skillType.ToString() + "\nLv." + character.skillLevel.ToString() + "(" + character.skillPoint.ToString() + ")";
    }
    public void SetTarget2Character(Character character)
    {
        Target2Rarity.text = "��" + character.rarity.ToString();
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
        Target2Skill.text = "�X�L���F\n" + character.skillType.ToString() + "\nLv." + character.skillLevel.ToString() + "(" + character.skillPoint.ToString() + ")";
    }
    public void SetResultCharacter(Character character, int skillType1, int skillType2)
    {
        ResultRarity.text = "��" + character.rarity.ToString();
        ResultHitPoint.text = "�̗́F" + character.maxHp.ToString();
        ResultPower.text = "�́F" + character.power.ToString();
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
        ResultAttackSpd.text = "�U���F" + tempstring;
        ResultSkill.text = "�X�L���F\n" + skillType1+"/"+ skillType2+ "\nLv." + character.skillLevel.ToString() + "(" + character.skillPoint.ToString() + ")";
    }
    public void ActivateDisplay()
    {
        mergeDisplay.SetActive(true);
    }
    public void DeactivateDisplay()
    {
        mergeDisplay.SetActive(false);
    }
    public void UpdateCursol(int cursol)
    {
        Cursol.GetComponent<RectTransform>().position = CursolPosition[cursol].position;
    }
}
