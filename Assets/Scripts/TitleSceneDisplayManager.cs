using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneDisplayManager : MonoBehaviour
{
    public static TitleSceneDisplayManager Instance;
    [SerializeField] List<RectTransform> cursolPosition;
    [SerializeField] GameObject cursolObject;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateCursol(1);
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCursol(int cursol)
    {
        cursolObject.GetComponent<RectTransform>().position = cursolPosition[cursol-1].position;
    }
}
