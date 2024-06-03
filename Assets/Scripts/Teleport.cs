using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public GameObject Portal;
    public GameObject Player;

    public Transform teleportpoint;

	void Start ()
    {
        
	}

	void Update ()
    {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (Vector2.Distance(transform.position, collision.transform.position) > 0.2f)
        {
            Player.transform.position = new Vector2(teleportpoint.position.x,teleportpoint.position.y);
            //Debug.Log("TELEPORT");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   //Set the tag Player!
        {
            //Debug.Log("EXIT");
        }
    }
}
