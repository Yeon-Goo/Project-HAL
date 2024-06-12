using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Portal : MonoBehaviour
{
    public GameObject portal;
    
     void Update()
    {
        if (GameObject.Find("king_slime") == null)
        {
            if (portal != null)
            {
                portal.SetActive(true);
                Debug.Log("Stage0_Clear_Portal has been activated.");
            }
    }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             if (SceneManager.GetActiveScene().name == "Stage0")
             {
                Vector3 targetposition = new Vector3(4,-25,0);
                other.transform.position = targetposition;
                SoundManager.Instance.PlayMusic("KingSlimeMusic");
            }
        }
    }
}
