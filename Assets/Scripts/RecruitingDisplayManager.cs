using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruitingDisplayManager : MonoBehaviour
{
    public static RecruitingDisplayManager Instance;
    [SerializeField] TextMeshProUGUI NextButtonText;
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
    public void UpdateNextButtonText()
    {

    }
    public void UpdateStatusCursol(float angle)
    {

    }
    public void UpdateSkillCursol(float angle)
    {

    }
}
