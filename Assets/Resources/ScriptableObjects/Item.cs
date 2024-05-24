using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Item")]

// Item의 속성을 정의하는 Sciprtable Object 클래스
public class Item : ScriptableObject
{
    [SerializeField]
    // Item의 이름
    private string objectName;
    [SerializeField]
    // Item의 sprite
    private Sprite sprite;
    [SerializeField]
    // Item의 수량 (default : 1)
    private int quantity;
    [SerializeField]
    // Item의 stack 여부 (default : false)
    private bool stackable;
    [SerializeField]
    // Item의 타입
    private ItemTypeEnum itemType;

    // ** 여기 private enum으로 하면 왜 GetItemType()에서 오류가 나는지 모르겠음 **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH
    }
    

    public Item(Item item)
    {
        this.objectName = item.objectName;
        this.quantity = item.quantity;
        this.stackable = item.stackable;
        this.itemType = item.itemType;
    }

    public Item(Item item, int quantity)
    {
        this.objectName = item.objectName;
        this.quantity = quantity;
        this.stackable = item.stackable;
        this.itemType = item.itemType;
    }

    public string ObjectName
    {
        get { return this.objectName; }
        set { this.objectName = value; }
    }

    public Sprite Sprite
    {
        get { return this.sprite; }
        set { this.sprite = value; }
    }

    public int Quantity
    {
        get { return this.quantity; }
        set { this.quantity = value; }
    }

    public bool Stackable
    {
        get { return this.stackable; }
        set { this.stackable = value; }
    }

    public ItemTypeEnum ItemType
    {
        get { return this.itemType; }
        set { this.itemType = value; }
    }
}
