using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPMP : MonoBehaviour
{
    private int MaxHP = 10;
    private int MaxMP = 20;

    private int CurHP = 10;
    private int CurMP = 0;

    public void HPDamaged(int a)
    {
        CurHP -= a;
        //������ ȣ��� �޼���
    }

    public void MPUse(int a)
    {
        CurMP -= a;
        //���� �����ϸ� ȣ��� �޼���
    }

}
