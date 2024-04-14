using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int mass; // 現在のマス。0 が自陣、 1 から GameManager.Instance.LaneLength までが道、 GameManager.Instance.LaneLength+1 マス目が敵陣
    public int lane; // 現在の列。0 が自陣、左から 1,,,MaxLaneまで。
    public int encampment; // 自陣の座標。0 がなし、1 から 12 までが ally 、13 から 18 までが控え
    public int maxHp; // 最大体力
    public int nowHp; // 現在の体力
    public int power; // 攻撃力
    public int attackSpd; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
    public int skillType; // スキルの種類
    public int skillTurn; // スキルのリロード
    public int skillnowTurn; // スキルの残りターン数。
    public int skillPoint; // スキルポイント、０～１６で表現
    public int skillLevel; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
    public int rarity; // レアリティ、☆１～５で表現
    public int reviveTurn; //復活までにあと何ターン必要か
    public int reviveMaxTurn; //復活までに何ターン必要か
    public bool exists;//実在するか否か
    public enum CharaState
    {
        None,
        Death, //Ally限定、死んで復活街、配置転換や売却などは不可能
        Ally, //Ally限定、出撃前、売ったり配置転換ができる
        Reserve, //控え、売ったり配置転換ができる
        Waiting, //Ally限定、出撃中(mass,lane には同じものを共有)
        Frontline, //戦線(encampment に待ち先を記入)
        Goal,//いるんかわからん
        Enemy
    }
    public CharaState charaState; //キャラクターの状態
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetStatus(Character targetCharacter, bool keepPosition = false)
    {
        if (!keepPosition)
        {
            mass = targetCharacter.mass; // 現在のマス。0 が自陣、 1 から GameManager.Instance.LaneLength までが道、 GameManager.Instance.LaneLength+1 マス目が敵陣
            lane = targetCharacter.lane; // 現在の列。0 が自陣、左から 1,,,MaxLaneまで。
            encampment = targetCharacter.encampment; // 自陣の座標。0 がなし、1 から 12 までが ally 、13 から 18 までが控え
        }
        maxHp = targetCharacter.maxHp; // 最大体力
        nowHp = targetCharacter.nowHp; // 現在の体力
        power = targetCharacter.power; // 攻撃力
        attackSpd = targetCharacter.attackSpd; // 攻撃速度、実際の数値ではなく4倍したもので管理、攻撃速度が 1.25 であれば 5 として管理する
        skillType = targetCharacter.skillType; // スキルの種類
        skillTurn = targetCharacter.skillTurn; // スキルのリロード
        skillnowTurn = targetCharacter.skillnowTurn; // スキルの残りターン数。
        skillPoint = targetCharacter.skillPoint; // スキルポイント、０～１６で表現
        skillLevel = targetCharacter.skillLevel; // スキルレベル、スキルポイントを３で割った商(切り捨て)、０～５で表現
        rarity = targetCharacter.rarity; // レアリティ、☆１～５で表現
        exists = targetCharacter.exists;
        charaState = targetCharacter.charaState;
        reviveTurn = targetCharacter.reviveTurn; //復活までにあと何ターン必要か
        reviveMaxTurn = targetCharacter.reviveMaxTurn; //復活までに何ターン必要か
    }
    public void SetExists(bool exists)
    {
        this.exists = exists;
    }
    public void Revive()
    {
        if ((reviveTurn > 0) || (charaState == CharaState.Death))
        {
            print("Revive error!! invalid status reviveturn:" + reviveTurn + " Charastate:" + charaState);
            return;
        }
        reviveTurn = 0;
        UpdateCharacterStatusInCamp();
    }
    public void UpdateCharacterStatusInCamp()
    {
        if (encampment <= 0)
        {
            print("UpdateCharacterStatus error!! invalid encampment:" + encampment);
            return;
        }
        if (encampment <= 12)
        {
            charaState = CharaState.Ally;
        }
        else
        {
            charaState = CharaState.Reserve;
        }
    }
}
