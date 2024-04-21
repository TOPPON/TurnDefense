using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStageDisplayManager : MonoBehaviour
{
    public static BattleStageDisplayManager Instance;
    [SerializeField] GameObject LaneObject;
    [SerializeField] GameObject MassObject;
    [SerializeField] GameObject FirstLineObject;
    [SerializeField] List<RectTransform> CursolPosition;
    [SerializeField] List<DisplayCharacter> CharaDisplayObject;//�K�v������Βǉ�����
    [SerializeField] GameObject CursolObject;
    [SerializeField] GameObject NormalCursolObject;
    [SerializeField] GameObject BattleStageObject;
    [SerializeField] GameObject DisplayCharacterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //SetLaneAndMass(5,8);
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLaneAndMass(int laneCount, int laneLength)
    {
        for (int i = 0; i < laneCount; i++)
        {
            GameObject newLane = Instantiate(LaneObject);
            newLane.transform.parent = FirstLineObject.transform;
            newLane.GetComponent<RectTransform>().localPosition = new Vector3(1440.0f / (laneCount + 1) * (i + 1) - 720, 0);
            RectTransform[] edgeMasses = newLane.GetComponentsInChildren<RectTransform>();
            RectTransform startTransform = new RectTransform();
            RectTransform goalTransform = new RectTransform();
            for (int j = 0; j < edgeMasses.Length; j++)
            {
                switch (edgeMasses[j].gameObject.name)
                {
                    case "StartMass":
                        startTransform = edgeMasses[j];
                        break;
                    case "GoalMass":
                        goalTransform = edgeMasses[j];
                        break;
                }
            }
            //�X�^�[�g�}�X��o�^
            CursolPosition.Add(startTransform);
            for (int j = 0; j < laneLength; j++)
            {
                GameObject newMass = Instantiate(MassObject);
                newMass.transform.parent = newLane.transform;
                newMass.GetComponent<RectTransform>().localPosition = new Vector3(0, 700.0f / (laneLength + 1) * (j + 1) - 350);
                CursolPosition.Add(newMass.GetComponent<RectTransform>());
            }
            //�S�[���}�X��o�^
            CursolPosition.Add(goalTransform);
        }
    }
    public void UpdateCursol(int cursol)
    {
        CursolObject.GetComponent<RectTransform>().position = CursolPosition[cursol + 21].position;
    }
    public void ActivateNormalCursol(int cursol)
    {
        print(cursol);
        NormalCursolObject.SetActive(true);
        NormalCursolObject.GetComponent<RectTransform>().position = CursolPosition[cursol + 21].position;
    }
    public void DeactivateNormalCursol()
    {
        NormalCursolObject.SetActive(false);
    }
    public Vector3 GetCursolPosition(int cursol)
    {
        return CursolPosition[cursol + 21].position;
    }
    public void RefreshCharacter(Character character)
    {

        int cursol = 0;
        switch (character.charaState)
        {
            case Character.CharaState.None:
                print("None���������Ƃ��Ă��܂�");
                break;
            case Character.CharaState.Ally:
            case Character.CharaState.Reserve://�����̐w�n�ɂ���ꍇ
            case Character.CharaState.Waiting://�����̐w�n�ɂ���ꍇ
            case Character.CharaState.Death:
                print("encampment:" + character.encampment);
                cursol = BattleStageManager.Instance.GetCursolByEncampment(character.encampment);
                break;
            case Character.CharaState.Frontline:
            case Character.CharaState.Goal:
            case Character.CharaState.Enemy:
                cursol = BattleStageManager.Instance.GetCursolByFrontlineLaneAndMass(character.lane, character.mass);
                break;
        }
        print("cursol:" + cursol);
        if (character.exists)
        {
            for (int i = CharaDisplayObject.Count - 1; i >= 0; i--)
            {
                DisplayCharacter dc = CharaDisplayObject[i];
                if (dc.cursol == cursol)
                {
                    //���߂����`���ł��邩�`�F�b�N
                    if (dc.charaType == character.skillType && dc.displayCharaState == character.charaState)
                    {
                        //�����L�����N�^�[
                        if (dc.displayCharaState == Character.CharaState.Death)
                        {
                            if (dc.reviveTurn != character.reviveTurn)
                            {
                                //�����^�[�����X�V
                                dc.reviveTurn = character.reviveTurn;
                            }
                            else
                            {

                            }
                        }
                    }
                    return;
                }
            }
            //�X�L���^�C�v�Ə�Ԃ����킹�ĐV�K�쐬
            DisplayCharacter newDisplayChara;
            //switch();
            newDisplayChara = Instantiate(DisplayCharacterPrefab).GetComponent<DisplayCharacter>();
            newDisplayChara.transform.parent = BattleStageObject.transform;
            newDisplayChara.SetAll(cursol, character.skillType, character.reviveTurn, character.reviveMaxTurn, character.rarity, character.charaState);
            CharaDisplayObject.Add(newDisplayChara);
        }
        else
        {
            print("CharaDisplayObjectCount:" + CharaDisplayObject.Count);
            for (int i = CharaDisplayObject.Count - 1; i >= 0; i--)
            {
                DisplayCharacter dc = CharaDisplayObject[i];
                if (dc.cursol == cursol)
                {
                    CharaDisplayObject.RemoveAt(i);
                    Destroy(dc.gameObject);
                }
            }
        }
    }
}
