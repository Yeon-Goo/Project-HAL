using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    // Item의 stack 여부 (default : false)
    private bool stackable;
    [SerializeField]
    // Item의 타입
    private ItemTypeEnum itemType;
    [SerializeField]
    private string prefabPath;

    // ** 여기 private enum으로 하면 왜 GetItemType()에서 오류가 나는지 모르겠음 **
    public enum ItemTypeEnum
    {
        COIN,
        HEALTH,
        STONE,
        GRASS
    }
    

    public Item(Item item)
    {
        this.objectName = item.ObjectName;
        this.sprite = item.Sprite;
        this.stackable = item.Stackable;
        this.itemType = item.ItemType;
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

    public string PrefabPath
    {
        get { return this.prefabPath; }
        set { this.prefabPath = value; }
    }
}
