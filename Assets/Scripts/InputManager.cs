using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
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
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.Instance.PushAButton();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameManager.Instance.PushBButton();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameManager.Instance.PushLeftButton();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameManager.Instance.PushRightButton();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameManager.Instance.PushUpButton();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameManager.Instance.PushDownButton();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.Instance.PushStartButton();
        }
    }
}
