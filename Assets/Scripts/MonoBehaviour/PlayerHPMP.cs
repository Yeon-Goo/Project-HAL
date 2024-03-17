using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPMP : MonoBehaviour
{
    [SerializeField]
    private int MaxHP = 10;
    [SerializeField]
    private int MaxMP = 20;

    [SerializeField]
    private int CurHP = 10;
    [SerializeField]
    private int CurMP = 0;

    public void HPDamaged(int a)
    {
        CurHP -= a;
        //죽으면 호출될 메서드
    }

    public void MPUse(int a)
    {
        CurMP -= a;
        //마나 부족하면 호출될 메서드
    }

}
