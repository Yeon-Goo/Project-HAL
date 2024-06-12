using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObjects : MonoBehaviour
{
    [SerializeField]
    private Item item;
    [SerializeField]
    // Item의 수량 (default : 1)
    private int quantity = 1;

    private float time = 0;
    private Vector3 start_position = Vector3.zero;
    private Transform transform = null;

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

    void Start()
    {
        time = 0;
        transform = GetComponent<Transform>();
        start_position = transform.position;
    }

    private void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector3(start_position.x, start_position.y + 0.1f * Mathf.Sin(time * 60.0f * Mathf.PI * Mathf.Deg2Rad), start_position.z);
    }
}