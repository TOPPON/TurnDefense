using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchManager : MonoBehaviour
{
    public static MarchManager Instance;
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
    public void DecideNextAction(Character target)
    {
        //�s�������߂�B�s����4��ނŁAAttack,March,Skill,Waiting�B
        //��{�I��Attack��March��Waiting�ASkill�͔Y��
        int lane = target.lane;
        int mass = target.mass;
        //
    }
}
