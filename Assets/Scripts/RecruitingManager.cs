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
    public float statusRollingSpeed;
    public float skillRollingSpeed;
    public int statusTimer;
    public int skillTimer;
    public int[] statusRouletteNumber = new int[24];
    public int[] skillRouletteNumber = new int[4];
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
        statusTimer = 100;
        skillTimer = 100;
        RecruitingDisplayManager.Instance.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        switch (recruitingState)
        {
            case RecruitingState.BeforeStatus:
                statusRollingSpeed = Random.Range(10f, 15f);
                recruitingState = RecruitingState.StatusRolling;
                break;
            case RecruitingState.StatusRolling:
                if (statusTimer > 0)
                {
                    statusTimer--;
                    statusAngle += statusRollingSpeed;
                    RecruitingDisplayManager.Instance.UpdateStatusCursol(statusAngle);
                    statusRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                else
                {
                    recruitingState = RecruitingState.AfterStatus;
                }
                break;
            case RecruitingState.AfterStatus:
                recruitingState = RecruitingState.BeforeSkill;
                break;
            case RecruitingState.BeforeSkill:
                skillRollingSpeed = Random.Range(10f, 15f);
                recruitingState = RecruitingState.SkillRolling;
                break;
            case RecruitingState.SkillRolling:
                if (skillTimer > 0)
                {
                    skillTimer--;
                    skillAngle += statusRollingSpeed;
                    RecruitingDisplayManager.Instance.UpdateSkillCursol(skillAngle);
                    skillRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                else
                {
                    recruitingState = RecruitingState.AfterSkill;
                }
                break;
            case RecruitingState.AfterSkill:
                break;
        }
    }
    public void PushAButton()
    {
        switch (recruitingState)
        {
            case RecruitingState.BeforeStatus:
                recruitingState = RecruitingState.AfterStatus;
                for (int i = 0; i < statusTimer; i++)
                {
                    statusAngle += statusRollingSpeed;
                    statusRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                RecruitingDisplayManager.Instance.UpdateStatusCursol(statusAngle);
                break;
            case RecruitingState.StatusRolling:
                recruitingState = RecruitingState.AfterStatus;
                for (int i = 0; i < statusTimer; i++)
                {
                    skillAngle += statusRollingSpeed;
                    skillRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                RecruitingDisplayManager.Instance.UpdateSkillCursol(skillAngle);
                break;
            case RecruitingState.AfterStatus:
                break;
            case RecruitingState.BeforeSkill:
                recruitingState = RecruitingState.AfterSkill;
                for (int i = 0; i < skillTimer; i++)
                {
                    skillAngle += statusRollingSpeed;
                    skillRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                RecruitingDisplayManager.Instance.UpdateSkillCursol(skillAngle);
                break;
            case RecruitingState.SkillRolling:
                recruitingState = RecruitingState.AfterSkill;
                for (int i = 0; i < skillTimer; i++)
                {
                    skillAngle += statusRollingSpeed;
                    skillRollingSpeed *= Random.Range(0.9f, 0.95f);
                }
                RecruitingDisplayManager.Instance.UpdateSkillCursol(skillAngle);
                break;
            case RecruitingState.AfterSkill:
                break;
        }
    }
}
