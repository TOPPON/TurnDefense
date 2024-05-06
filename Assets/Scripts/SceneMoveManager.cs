using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    public static SceneMoveManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            print("hogegeho");
        }
        else Destroy(gameObject);
    }
    public int difficulty = 1;
    int titleCursol = 1;//“ïˆÕ“x‚Æˆê
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "BattleScene":
                    break;
                case "TitleScene":
                    switch (titleCursol)
                    {
                        case 1:
                            ChooseDifficult1Button();
                            break;
                        case 2:
                            ChooseDifficult2Button();
                            break;
                        case 3:
                            ChooseDifficult3Button();
                            break;
                    }
                    break;
                case "GameClearScene":
                    MoveToTitleScene();
                    break;
                case "GameOverScene":
                    MoveToTitleScene();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "BattleScene":
                    break;
                case "TitleScene":
                    if (titleCursol > 1)
                    {
                        titleCursol--;
                        TitleSceneDisplayManager.Instance.UpdateCursol(titleCursol);
                    }
                    break;
                case "GameClearScene":
                    break;
                case "GameOverScene":
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "BattleScene":
                    break;
                case "TitleScene":
                    if (titleCursol < 3)
                    {
                        titleCursol++;
                        TitleSceneDisplayManager.Instance.UpdateCursol(titleCursol);
                    }
                    break;
                case "GameClearScene":
                    break;
                case "GameOverScene":
                    break;
            }
        }
    }
    public void MoveToGameClearScene()
    {
        SceneManager.LoadScene("GameClearScene");
    }
    public void MoveToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void MoveToBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void ChooseDifficult1Button()
    {
        difficulty = 1;
        MoveToBattleScene();
    }
    public void ChooseDifficult2Button()
    {
        difficulty = 2;
        MoveToBattleScene();
    }
    public void ChooseDifficult3Button()
    {
        difficulty = 3;
        MoveToBattleScene();
    }
}
