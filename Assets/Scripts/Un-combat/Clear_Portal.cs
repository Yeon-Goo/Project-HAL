using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear_Portal : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             if (SceneManager.GetActiveScene().name == "Stage0")
             {
                Vector3 Clear = new Vector3(-103,11,0);
                other.transform.position = Clear;
             }
        }
    }
        
    
}
