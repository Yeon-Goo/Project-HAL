using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Card", fileName = "Card")]

// Card�� �Ӽ��� �����ϴ� Sciprtable Object Ŭ����
public class Card : ScriptableObject
{
    // Card�� �̸�
    public string objectName;
    // Card�� ���� ��ȣ
    public int num;
    // Card�� ����
    public int level;
    // Card�� �ؽ�Ʈ
    public TMP_Text text;
    // Card�� sprite
    public Image image;
    // Card�� �ر� ����
    public bool is_unlocked;

    public enum CardNameEnum
    {
    }
}
