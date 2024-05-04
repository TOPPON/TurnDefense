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
    [SerializeField] Image reviveRollingDisplay;
    [SerializeField] TextMeshProUGUI reviveTurnText;
    [SerializeField] TextMeshProUGUI rarityText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetAll(int cursol,int charaType,int reviveTurn,int reviveMaxTurn,int rarity,Character.CharaState charaState)
    {
        this.cursol = cursol;
        gameObject.transform.position = BattleStageDisplayManager.Instance.GetCursolPosition(cursol);
        gameObject.GetComponent<Image>().sprite = GetDisplayCharacterByType(charaType);
        rarityText.text = "星"+rarity.ToString();
        waitingDisplay.SetActive(false);
        reviveDisplay.SetActive(false);
        switch(charaState)
        {
            case Character.CharaState.Waiting:
                print("waiting");
                waitingDisplay.SetActive(true);
                break;
            case Character.CharaState.Death:
                if (reviveTurn>0)//�c��^�[����1�ȏ�̂Ƃ�
                {
                    reviveDisplay.SetActive(true);
                    reviveRollingDisplay.fillAmount = 1.0f * reviveTurn / reviveMaxTurn;
                    reviveTurnText.text = reviveTurn.ToString();
                }
                break;
        }
        //print("reviveTurn:"+reviveTurn);
            
    }
    public static Sprite GetDisplayCharacterByType(int charaType)
    {
        print("charaType:" +charaType);
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
        print("error!!!! invalid charaType:"+charaType);
        return Resources.Load<Sprite>("Sprites/Black");
    }
}
