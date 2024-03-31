using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int mass; // 現在のマス。0 が自陣、 1 から GameManager.Instance.LaneLength までが道、 GameManager.Instance.LaneLength+1 マス目が敵陣
    public int maxHp; // 最大体力
    public int nowHp; // 現在の体力
    public int power; // 攻撃力
    public int attackSpd; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
    public int skillType; // スキルの種類
    public int skillTurn; // スキルのリロード
    public int skillnowTurn; // スキルの残りターン数。
    public int skillPoint; // スキルポイント、０～１６で表現
    public int skillLevel; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
    public int rarerity; // レアリティ、☆１～５で表現
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
