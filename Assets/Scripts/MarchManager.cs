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
        //行動を決める。行動は4種類で、Attack,March,Skill,Waiting。
        //基本的にAttack＞March＞Waiting、Skillは悩む
        int lane = target.lane;
        int mass = target.mass;
        //
    }
}
