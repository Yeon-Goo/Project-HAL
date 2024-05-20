using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeOne : MonoBehaviour
{
    public void changeOne(){
        SceneManager.LoadScene("Stage1");
    }
}
