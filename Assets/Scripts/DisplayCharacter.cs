using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCharacter : MonoBehaviour
{
    public int cursol; //BattleStage の cursol に対応する
    public int charaType; //一旦キャラのスキルと同一
    public int reviveTurn; //復活までにあと何ターン必要か
    public int reviveMaxTurn; //復活までに何ターン必要か
    public Character.CharaState displayCharaState;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
