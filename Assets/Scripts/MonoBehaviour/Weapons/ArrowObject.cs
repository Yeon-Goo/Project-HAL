using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        rb.velocity = transform.right * speed;
    }

    void Update()
    {
        // destroy when distance > 10
        if (Vector2.Distance(startPosition, rb.position) > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("enemy"))
        {
            //need to fix to destory -> damage
            Destroy(hitInfo.gameObject);
            Destroy(gameObject);
        }
    }
}
