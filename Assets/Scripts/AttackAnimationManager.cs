using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    public static AttackAnimationManager Instance;

    //���̃Q�[����spd=4 (1.00) �̂Ƃ���5�b�ԂŌ܉�U���ł���悤�ɂ������B
    //20/spd ��U�����ł���d�g��
    //200�Ń}�b�N�X�ɂȂ�悤�ɂ��Ă��C������
    const float UPDATE_TIME = 0.02f;//��R�}�����b�ȏ�P�ʂŋ��߂邩
    float updateTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    void Activate()
    {
        AttackAnimationDisplayManager.Instance.ActivateDisplay();
    }
    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer > UPDATE_TIME)
        {
            updateTimer = 0;
        }
    }
}
