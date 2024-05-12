using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    protected PlayerEntity playerEntity;
    protected float lastUseTime;
    //0~11 Archer 12~23 Gunner



    // 카드 번호에 따른 스킬 실행
    public virtual int Skill(int num, int level)
    {
        //weapon별로 구현
        return 0;
    }



    public virtual void BaseAttack()
    {
        //weapon별로 구현
    }



    //기본 공격이 가능한지 반환
    public virtual bool BaseAttackAble()
    {
        //weapon별로 구현
        return false;
    }



    //스킬 사용에 필요한 마나 반환
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
