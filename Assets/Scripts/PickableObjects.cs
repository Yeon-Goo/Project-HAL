using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjects : MonoBehaviour
{
    [SerializeField]
    private Item item;
    [SerializeField]
    // Item의 수량 (default : 1)
    private int quantity = 1;

    public PickableObjects(Item item)
    {
        Debug.Log("생성자");
        this.item = new Item(item);
        this.quantity = 1;
    }

    public PickableObjects(Item item, int quantity)
    {
        this.item = new Item(item);
        this.quantity = quantity;   
    }

    public Item Item
    {
        get { return this.item; }
        set { this.item = value; }
    }

    public int Quantity
    {
        get { return this.quantity; }
        set { this.quantity = value; }
    }
}
