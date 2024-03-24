using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    // ī�� ��ȣ�� ���� ��ų ����
    public virtual void Skill(int num, int level)
    {
        // ���⼭ ������ ���ǹ��� ����Ͽ� num�� ���� �ٸ� ��ų�� ������ �� �ֽ��ϴ�.
        // ����:
        switch (num)
        {
            case 0:
                Debug.Log("Card 0 activated.");
                break;
            case 1:
                Debug.Log("Card 1 activated.");
                break;
            // �߰����� ��ų ���̽�
            default:
                Debug.Log($"Card {num} not implemented.");
                break;
        }
    }
}
