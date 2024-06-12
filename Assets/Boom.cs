using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject CarpetPrefab; // Carpet 프리팹을 할당하기 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAndSpawnCarpet());
    }

    IEnumerator DestroyAndSpawnCarpet()
    {
        yield return new WaitForSeconds(1.0f); // 1초 대기
        Instantiate(CarpetPrefab, transform.position, Quaternion.identity); // Boom 위치에 Carpet 생성
        Destroy(gameObject); // Boom 오브젝트 파괴
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}