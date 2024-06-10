using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolAnimation : MonoBehaviour
{
    //�X�L���̃A�C�R����o�g���̃A�C�R����\������p�̃N���X�B��{�I�ɃA�j���[�V�����ŏ���ɏ�������̂̑z��
    // Start is called before the first frame update
    public enum SymbolType
    {
        BattleStart,//�傫���Ȃ�
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
    public void Occur(SymbolType symbolType, Vector3 potision, float lifetime)//�}�X�ȊO�ɂ��\�����镝������\��������̂�
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
