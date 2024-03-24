using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BlockMaker의 경우, Map에 정해진 할당량보다 적어질 경우, 일정 시간 이후 가능한 랜덤한 맵 위치에서 재생성
//차후 glass와 stone의 종류가 늘어날 경우, Maker를 따로 만들 예정


public class BlockMaker : MonoBehaviour
{
    
    public GameObject GlassPrefab; // GlassPrefab
    public GameObject StonePrefab; // StonePrefab
    public int Test_GlassSpawnCount = 5; // 테스트용 GlassPrefab의 초기 생성 개수, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    public int Test_StoneSpawnCount = 5; // 테스트용 StonePrefab의 초기 생성 개수, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    public float Test_respawnTime = 5f; // 테스트용 재생성 시간, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정 
    public float Test_contactDuration = 2f; // 테스트용 자원 채집하는데 걸리는 시간, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정 

    void Start()
    {
        // 초기 자원 생성
        SpawnResources();
    }

    void SpawnResources()
    {
        for (int i = 0; i < Test_GlassSpawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject glass = Instantiate(GlassPrefab, spawnPosition, Quaternion.identity);
            glass.tag = "Glass"; // GlassPrefab의 태그 설정;
        }
        // GlassPrefab 초기 생성
        
        for (int i = 0; i < Test_StoneSpawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
             GameObject stone = Instantiate(StonePrefab, spawnPosition, Quaternion.identity);
            stone.tag = "Stone"; // StonePrefab의 태그 설정
        }
        // StonePrefab 초기 생성
    }

    Vector3 GetRandomSpawnPosition()
    {
        // 랜덤한 위치 계산
        float x = Random.Range(-10f, 10f); // 위치는 Map_size에 따라 달라질 예정
        float y = Random.Range(-5f, 5f); // // 위치는 Map_size에 따라 달라질 예정
        return new Vector3(x, y, 0f);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartResourceCollection());
        }
    }

    IEnumerator StartResourceCollection()
    {
        float startTime = Time.time;
        while (Time.time - startTime < Test_contactDuration)
        {
            yield return null;
        }

        Destroy(gameObject);
        StartCoroutine(RespawnResources());
    }

    IEnumerator RespawnResources()
    {
        yield return new WaitForSeconds(Test_respawnTime);
        //부족한 glassPrefeb 개수만큼 생성
         int missingGlassResources = Test_GlassSpawnCount - GameObject.FindGameObjectsWithTag("Glass").Length;
        SpawnGlassResources(missingGlassResources);

        // 부족한 StonePrefeb 개수만큼 생성
        int missingStoneResources = Test_StoneSpawnCount - GameObject.FindGameObjectsWithTag("Stone").Length;
        SpawnStoneResources(missingStoneResources);
    }
    void SpawnGlassResources(int count)
    {
        // GlassPrefab 생성
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
           GameObject glass = Instantiate(GlassPrefab, spawnPosition, Quaternion.identity);
            glass.tag = "Glass"; // 없어진 후 새로 생성되는 GlassPrefab의 태그 설정
        }
    }

    void SpawnStoneResources(int count)
    {
        // StonePrefab 생성
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject stone = Instantiate(StonePrefab, spawnPosition, Quaternion.identity);
            stone.tag = "Stone"; // 없어진 후 새로 생성되는 StonePrefab의 태그 설정
        }
    }
}

