using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingManager : MonoBehaviour
{
    public static RecruitingManager Instance;
    public enum RecruitingState
    {
        BeforeStatus,
        StatusRolling,
        AfterStatus,
        BeforeSkill,
        SkillRolling,
        AfterSkill
    }
    public RecruitingState recruitingState;
    public float statusAngle;
    public float skillAngle;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void Activate()
    {
        recruitingState = RecruitingState.BeforeStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushAButton()
    {

    }
}
