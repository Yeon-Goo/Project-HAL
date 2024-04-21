using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPoint : MonoBehaviour
{
    // 스폰시킬 객체의 prefab
    public GameObject prefab_to_spawn;
    // 스폰 사이의 대기 시간
    public float repeat_interval;

    // Start is called before the first frame update
    void Start()
    {
        prefab_to_spawn = Resources.Load<GameObject>("Prefabs/PlayerObject");

        if (prefab_to_spawn == null) Debug.Log("Spawn Object is null\n");

        if (repeat_interval > 0)
        {
            // arg_0 : 호출할 메서드, arg_1 : 최초 호출까지의 대기 시간, arg_2 : 호출 사이의 대기 시간
            InvokeRepeating("SpawnObject", 0.0f, repeat_interval);
        }
    }
    
    public GameObject SpawnObject()
    {
        if (prefab_to_spawn != null)
        {
            // arg_0 : 스폰 대상, arg_1 : 스폰 위치, arg_2 : 회전 여부 (Quaternion.identity = 회전 없음)
            //return Instantiate(prefab_to_spawn, transform.position, Quaternion.identity);
        }
        
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
