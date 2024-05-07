using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    protected PlayerEntity playerEntity;

    // 카드 번호에 따른 스킬 실행
    public virtual int Skill(int num, int level)
    {
        //weapon별로 구현
        return 0;
    }

    public virtual int GetMana(int cardnum)
    {
        //weapon별로 구현
        return 99;
    }

    void Start()
    {
        playerEntity = GameObject.Find("PlayerObject").GetComponent<PlayerEntity>();
    }
}
