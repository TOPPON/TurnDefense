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
    [SerializeField] List<DisplayCharacter> CharaDisplayObject;//必要があれば追加する
    [SerializeField] GameObject CursolObject;
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
            //スタートマスを登録
            CursolPosition.Add(startTransform);
            for (int j = 0; j < laneLength; j++)
            {
                GameObject newMass = Instantiate(MassObject);
                newMass.transform.parent = newLane.transform;
                newMass.GetComponent<RectTransform>().localPosition = new Vector3(0, 700.0f / (laneLength + 1) * (j + 1) - 350);
                CursolPosition.Add(newMass.GetComponent<RectTransform>());
            }
            //ゴールマスを登録
            CursolPosition.Add(goalTransform);
        }
    }
    public void UpdateCursol(int cursol)
    {
        print(cursol);
        CursolObject.GetComponent<RectTransform>().position = CursolPosition[cursol].position;
    }
    public void RefreshCharacter(Character character)
    {
        if (character.exists)
        {
            switch(character.charaState)
            {
                case Character.CharaState.Waiting:
                    break;
                case Character.CharaState.Reserve:
                    break;
                case Character.CharaState.Frontline:
                    break;
                case Character.CharaState.Goal:
                    break;
                case Character.CharaState.Enemy:
                    break;
            }
        }
        else
        {
            foreach(DisplayCharacter chara in CharaDisplayObject)
            {
                switch (character.charaState)
                {
                    case Character.CharaState.Waiting:
                        break;
                    case Character.CharaState.Reserve:
                        break;
                    case Character.CharaState.Frontline:
                        break;
                    case Character.CharaState.Goal:
                        break;
                    case Character.CharaState.Enemy:
                        break;
                }
            }
        }
    }
}
