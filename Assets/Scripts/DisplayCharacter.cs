using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayCharacter : MonoBehaviour
{
    public int cursol; //BattleStage の cursol に対応する
    public int charaType; //一旦キャラのスキルと同一
    public int reviveTurn; //復活までにあと何ターン必要か
    public int reviveMaxTurn; //復活までに何ターン必要か
    public int rarity; // レアリティ
    public Character.CharaState displayCharaState;
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
        /*switch (charaType)
        {
            case 0:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/character_kishi_man_02_green_black");
                break;
            case 1:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/character_uranaishi_01");
                break;
        }*/
        rarityText.text = "星"+rarity.ToString();
        print("reviveTurn:"+reviveTurn);
        if (reviveTurn>0)//残りターンが1以上のとき
        {
            reviveDisplay.SetActive(true);
            reviveRollingDisplay.fillAmount = 1.0f * reviveTurn / reviveMaxTurn;
            reviveTurnText.text = reviveTurn.ToString();
        }
        else
        {
            reviveDisplay.SetActive(false);
        }
            
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
        }
        print("error!!!! invalid charaType:"+charaType);
        return Resources.Load<Sprite>("Sprites/Black");
    }
}
