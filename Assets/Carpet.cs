using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carpet : MonoBehaviour
{
    private int dmg = 1; // 데미지 값

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 3f); // 3초 후에 자기 자신을 파괴
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = coll.gameObject.GetComponent<PlayerEntity>();
            StartCoroutine(player.DamageEntity(dmg, 1.0f, this.gameObject));
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject); // 자기 자신 파괴
    }

    // Update is called once per fram
}