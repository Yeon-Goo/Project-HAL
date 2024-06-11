using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Portal : MonoBehaviour
{
    public GameObject portal;
    private GameObject boss;
     void Update()
    {
        boss = GameObject.Find("king_slime");
        if (boss == null)
        {
                portal.SetActive(true);  
            
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
