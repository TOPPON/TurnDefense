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

    public void RefreshCharacterStatusDisplay(Character target)
    {
        
    }
    public void RefreshCharacterMergeDisplay(Character target1,Character target2)
    {

    }
}
