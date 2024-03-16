using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

// Item의 속성을 정의하는 Sciprtable Object 클래스
public class Item : ScriptableObject
{
    // Item의 이름
    private string objectName;
    // Item의 sprite
    private Sprite sprite;
    // Item의 수량 (default : 1)
    private int quantity;
    // Item의 stack 여부 (default : false)
    private bool stackable;

    // ** 여기 private enum으로 하면 왜 GetItemType()에서 오류가 나는지 모르겠음 **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH
    }
    // Item의 타입
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
