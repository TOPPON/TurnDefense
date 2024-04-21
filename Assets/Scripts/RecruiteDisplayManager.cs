using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruiteDisplayManager : MonoBehaviour
{
    public static RecruiteDisplayManager Instance;
    [SerializeField] TextMeshProUGUI NextButtonText;
    [SerializeField] GameObject recruiteDisplay;
    [SerializeField] GameObject[] statusSpots;
    [SerializeField] GameObject[] skillSpots;
    [SerializeField] GameObject[] skillSpotsChara;
    [SerializeField] GameObject[] skillSpotsCharaImage;
    [SerializeField] GameObject HpBoard;
    [SerializeField] GameObject HpBoardText;
    [SerializeField] GameObject AtkBoard;
    [SerializeField] GameObject AtkBoardText;
    [SerializeField] GameObject SpdBoard;
    [SerializeField] GameObject SpdBoardText;
    [SerializeField] GameObject SkillBoard;
    [SerializeField] GameObject SkillBoardText;
    [SerializeField] List<RectTransform> CursolPosition;
    [SerializeField] GameObject CursolObject;
    public Color hpColor;
    public Color atkColor;
    public Color spdColor;
    public Color skillColor;
    [SerializeField] GameObject StatusArrow;
    [SerializeField] GameObject SkillArrow;
    public int[] skillRouletteType = new int[5];
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
    public void ActivateDisplay()
    {
        recruiteDisplay.SetActive(true);
        UpdateCursol(0);
    }
    public void DeactivateDisplay()
    {
        recruiteDisplay.SetActive(false);
    }
    public void SetStatusRouletteWheel(int hp, int atk, int spd, int skill)
    {
        if (hp + atk + spd + skill != 24)
        {
            print("error!sum of roulette mass is invalid sum:" + (hp + atk + spd + skill).ToString());
            return;
        }
        //各マスの色を変更していく
        int wheelCursol = 0;
        for (int i = 0; i < 24; i++)
        {
            if (i < hp)
            {
                statusSpots[i].GetComponent<Image>().color = hpColor;
            }
            else if (i < hp + atk)
            {

                statusSpots[i].GetComponent<Image>().color = atkColor;
            }
            else if (i < hp + atk + spd)
            {

                statusSpots[i].GetComponent<Image>().color = spdColor;
            }
            else if (i < hp + atk + spd + skill)
            {
                statusSpots[i].GetComponent<Image>().color = skillColor;
            }
        }

        //各タイトルを設置
        wheelCursol = 0;
        if (hp == 0)
        {
            HpBoard.SetActive(false);
        }
        else
        {
            HpBoard.SetActive(true);
            HpBoard.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -wheelCursol * 15 - hp * 7.5f);
            HpBoardText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, wheelCursol * 15 + hp * 7.5f);
            wheelCursol += hp;
        }


        if (atk == 0)
        {
            AtkBoard.SetActive(false);
        }
        else
        {
            AtkBoard.SetActive(true);
            AtkBoard.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -wheelCursol * 15 - atk * 7.5f);
            AtkBoardText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, wheelCursol * 15 + atk * 7.5f);
            wheelCursol += atk;
        }


        if (spd == 0)
        {
            SpdBoard.SetActive(false);
        }
        else
        {
            SpdBoard.SetActive(true);
            SpdBoard.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -wheelCursol * 15 - spd * 7.5f);
            SpdBoardText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, wheelCursol * 15 + spd * 7.5f);
            wheelCursol += spd;
        }


        if (skill == 0)
        {
            SkillBoard.SetActive(false);
        }
        else
        {
            SkillBoard.SetActive(true);
            SkillBoard.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -wheelCursol * 15 - skill * 7.5f);
            SkillBoardText.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, wheelCursol * 15 + skill * 7.5f);
            wheelCursol += skill;
        }
    }
    public void SetSkillRouletteWheel(int skillAType, int skillBType, int skillCType, int skillDType, int skillEType = -1)
    {
        skillRouletteType[0] = skillAType;
        skillRouletteType[1] = skillBType;
        skillRouletteType[2] = skillCType;
        skillRouletteType[3] = skillDType;
        skillRouletteType[4] = skillEType;
        if (skillEType == -1)
        {
            for (int i = 0; i < 4; i++)
            {
                skillSpots[i].GetComponent<Image>().fillAmount = 0.25f;
                skillSpots[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -i * 90);
                skillSpotsChara[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -i * 90 - 45);
                skillSpotsCharaImage[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, i * 90 + 45);
                skillSpotsCharaImage[i].GetComponent<Image>().sprite = DisplayCharacter.GetDisplayCharacterByType(skillRouletteType[i]);
            }
            skillSpots[4].SetActive(false);
            skillSpotsChara[4].SetActive(false);

        }
        else
        {
            skillSpots[4].SetActive(true);
            skillSpotsChara[4].SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                skillSpots[i].GetComponent<Image>().fillAmount = 0.2f;
                skillSpots[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -i * 72);
                skillSpotsChara[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -i * 72 - 36);
                skillSpotsCharaImage[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, i * 72 + 36);
                skillSpotsCharaImage[i].GetComponent<Image>().sprite = DisplayCharacter.GetDisplayCharacterByType(skillRouletteType[i]);
            }
        }
    }
    public void UpdateNextButtonText(string newText)
    {
        NextButtonText.text = newText;
    }
    public void UpdateStatusArrow(float angle)
    {
        StatusArrow.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
    }
    public void UpdateSkillArrow(float angle)
    {
        SkillArrow.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
    }
    public void UpdateCursol(int index)
    {
        CursolObject.GetComponent<RectTransform>().position = CursolPosition[index].position;
    }
}
