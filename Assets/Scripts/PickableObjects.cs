using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjects : MonoBehaviour
{
    public Item item;
    // Item의 수량 (default : 1)
    private int quantity;

    public PickableObjects(Item item)
    {
        this.item = new Item(item);
        this.quantity = 1;
    }

    public PickableObjects(Item item, int quantity)
    {
        this.item = new Item(item);
        this.quantity = quantity;   
    }

    public int Quantity
    {
        get { return this.quantity; }
        set { this.quantity = value; }
    }
}
