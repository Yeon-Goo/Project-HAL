using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

// Item�� �Ӽ��� �����ϴ� Sciprtable Object Ŭ����
public class Item : ScriptableObject
{
    // Item�� �̸�
    public string objectName;
    // Item�� sprite
    public Sprite sprite;
    // Item�� ���� (default : 1)
    public int quantity;
    // Item�� stack ���� (default : false)
    public bool stackable;

    // ** ���� private enum���� �ϸ� �� GetItemType()���� ������ ������ �𸣰��� **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH
    }
    // Item�� Ÿ��
    public ItemTypeEnum itemType;

    public string GetName()
    {
        return objectName;
    }

    public Sprite GetSprite()
    {
        return sprite;
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

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public void SetStackable(bool stackable)
    {
        this.stackable = stackable;
    }

    public void SetItemType(ItemTypeEnum itemType)
    {
        this.itemType = itemType;
    }
}
