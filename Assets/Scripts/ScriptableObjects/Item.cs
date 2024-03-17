using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

// Item�� �Ӽ��� �����ϴ� Sciprtable Object Ŭ����
public class Item : ScriptableObject
{
    // Item�� �̸�
    private string objectName;
    // Item�� sprite
    private Sprite sprite;
    // Item�� ���� (default : 1)
    private int quantity;
    // Item�� stack ���� (default : false)
    private bool stackable;

    // ** ���� private enum���� �ϸ� �� GetItemType()���� ������ ������ �𸣰��� **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH
    }
    // Item�� Ÿ��
    private ItemTypeEnum itemType;

    public string GetName()
    {
        return objectName;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public bool GetStackable()
    {
        return stackable;
    }

    public ItemTypeEnum GetItemType()
    {
        return itemType;
    }
}
