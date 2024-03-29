using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

// Item의 속성을 정의하는 Sciprtable Object 클래스
public class Item : ScriptableObject
{
    // Item의 이름
    public string objectName;
    // Item의 sprite
    public Sprite sprite;
    // Item의 수량 (default : 1)
    public int quantity;
    // Item의 stack 여부 (default : false)
    public bool stackable;

    // ** 여기 private enum으로 하면 왜 GetItemType()에서 오류가 나는지 모르겠음 **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH
    }
    // Item의 타입
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
