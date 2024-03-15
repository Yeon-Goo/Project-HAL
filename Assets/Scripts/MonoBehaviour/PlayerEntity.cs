using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    private static string pickable_Objects = "Pickable_Objects";

    void Start()
    {
        // Need to fill something...
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_Objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if(hitObject != null)
            {
                print("Hit: " + hitObject.GetName());
                collision.gameObject.SetActive(false);
            }
        }
    }
}
