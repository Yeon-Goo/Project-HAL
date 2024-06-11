using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana_Object : MonoBehaviour
{
    private int dmg = 2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 3f);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = coll.gameObject.GetComponent<PlayerEntity>();
            StartCoroutine(player.DamageEntity(dmg, 1.0f, this.gameObject));
            Destroy(gameObject);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
