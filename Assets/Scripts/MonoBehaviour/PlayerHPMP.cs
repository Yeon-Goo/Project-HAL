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
        //������ ȣ��� �޼���
    }

    public void MPUse(int a)
    {
        CurMP -= a;
        //���� �����ϸ� ȣ��� �޼���
    }

}
