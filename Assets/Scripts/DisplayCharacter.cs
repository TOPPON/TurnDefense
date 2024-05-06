using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayCharacter : MonoBehaviour
{
    public int cursol; //BattleStage �� cursol �ɑΉ�����
    public int charaType; //��U�L�����̃X�L���Ɠ���
    public int reviveTurn; //�����܂łɂ��Ɖ��^�[���K�v��
    public int reviveMaxTurn; //�����܂łɉ��^�[���K�v��
    public int rarity; // ���A���e�B
    public Character.CharaState displayCharaState;
    [SerializeField] GameObject waitingDisplay;
    [SerializeField] GameObject reviveDisplay;
    [SerializeField] GameObject goalDisplay;
    [SerializeField] Image reviveRollingDisplay;
    [SerializeField] TextMeshProUGUI reviveTurnText;
    [SerializeField] TextMeshProUGUI rarityText;
    [SerializeField] GameObject hpDisplay;
    [SerializeField] Image hpBarGage;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI spdText;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetAll(int cursol, int charaType, int reviveTurn, int reviveMaxTurn, int rarity, Character.CharaState charaState, int nowHp, int maxHp, int atk, int spd)
    {
        this.cursol = cursol;
        gameObject.transform.position = BattleStageDisplayManager.Instance.GetCursolPosition(cursol);
        gameObject.GetComponent<Image>().sprite = GetDisplayCharacterByType(charaType);
        rarityText.text = "星" + rarity.ToString();
        waitingDisplay.SetActive(false);
        reviveDisplay.SetActive(false);
        goalDisplay.SetActive(false);
        hpDisplay.SetActive(false);
        switch (charaState)
        {
            case Character.CharaState.Waiting:
                waitingDisplay.SetActive(true);
                break;
            case Character.CharaState.Death:
                if (reviveTurn > 0)//�c��^�[����1�ȏ�̂Ƃ�
                {
                    reviveDisplay.SetActive(true);
                    reviveRollingDisplay.fillAmount = 1.0f * reviveTurn / reviveMaxTurn;
                    reviveTurnText.text = reviveTurn.ToString();
                }
                break;
            case Character.CharaState.Goal:
                goalDisplay.SetActive(true);
                break;
            case Character.CharaState.Ally:
            case Character.CharaState.Reserve:
            case Character.CharaState.Enemy:
            case Character.CharaState.Frontline:
                hpDisplay.SetActive(true);
                hpBarGage.fillAmount = 1.0f * nowHp / maxHp;
                hpText.text = nowHp + " / " + maxHp;
                break;
        }
        atkText.text = atk.ToString();
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
        spdText.text = tempstring;

    }
    public static Sprite GetDisplayCharacterByType(int charaType)
    {
        switch (charaType)
        {
            case 0:
                return Resources.Load<Sprite>("Sprites/character_kishi_man_02_green_black");
            case 1:
                return Resources.Load<Sprite>("Sprites/character_uranaishi_01");
            case 2:
                return Resources.Load<Sprite>("Sprites/character_kishi_woman_blue_gold");
            case 3:
                return Resources.Load<Sprite>("Sprites/character_murabito_child_02_man_blue");
            case 4:
                return Resources.Load<Sprite>("Sprites/character_oji_03_red_brown");
            case 5:
                return Resources.Load<Sprite>("Sprites/character_senshi_red");
            case 6:
                return Resources.Load<Sprite>("Sprites/character_shinpu_green");
            case 7:
                return Resources.Load<Sprite>("Sprites/character_tozoku_green");
            case 8:
                return Resources.Load<Sprite>("Sprites/character_heishi_armor_02_green");
            case 9:
                return Resources.Load<Sprite>("Sprites/character_yusha_01_red");
            case 10:
                return Resources.Load<Sprite>("Sprites/character_hime_child_white_gold");
            case 11:
                return Resources.Load<Sprite>("Sprites/character_yosei_02_blue");
            case 12:
                return Resources.Load<Sprite>("Sprites/character_mahotsukai_01_black");
        }
        print("error!!!! invalid charaType:" + charaType);
        return Resources.Load<Sprite>("Sprites/Black");
    }
}
