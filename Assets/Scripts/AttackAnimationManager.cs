using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationManager : MonoBehaviour
{
    public static AttackAnimationManager Instance;

    //このゲームはspd=4 (1.00) のときに5秒間で五回攻撃できるようにしたい。
    //20/spd 回攻撃ができる仕組み
    //200でマックスになるようにしてた気がする
    const float UPDATE_TIME = 0.02f;//一コマを何秒以上単位で求めるか
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
