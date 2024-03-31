using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleInformationDisplayManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI turnsText;
    [SerializeField] public TextMeshProUGUI clearConditionText;
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

    public void RefreshCharacterStatusDisplay(Character target)
    {
        
    }
    public void RefreshCharacterMergeDisplay(Character target1,Character target2)
    {

    }
}
