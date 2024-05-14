using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Start is called before the first frame update
    public void stop(){
        Time.timeScale = 0;
    }
    public void restart(){
        Time.timeScale = 1;
    }
}
