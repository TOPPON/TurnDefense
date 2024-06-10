using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolAnimation : MonoBehaviour
{
    //スキルのアイコンやバトルのアイコンを表示する用のクラス。基本的にアニメーションで勝手に消えるものの想定
    // Start is called before the first frame update
    public enum SymbolType
    {
        BattleStart,//大きくなる
        SkillActivate,
        Death,
    }
    SymbolType symbolType;
    float timer;
    float lifetime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (symbolType)
        {
            case SymbolType.BattleStart:
                if (timer <= lifetime / 2)
                {
                    transform.localScale = new Vector3(0.5f, 0.5f) + new Vector3(1f, 1f) * timer * 2 / lifetime;
                }
                else
                {
                    transform.localScale = new Vector3(1.5f, 1.5f);
                }
                break;
        }
        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
    public void Occur(SymbolType symbolType, Vector3 potision, float lifetime)//マス以外にも表現する幅がある可能性があるので
    {
        transform.transform.position = potision;
        this.lifetime = lifetime;
        switch (symbolType)
        {
            case SymbolType.BattleStart:
                transform.localScale = new Vector3(0.5f, 0.5f);
                break;
        }
    }
}
