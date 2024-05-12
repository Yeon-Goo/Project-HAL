using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BlockMaker의 경우, Map에 정해진 할당량보다 적어질 경우, 일정 시간 이후 가능한 랜덤한 맵 위치에서 재생성
//차후 glass와 stone의 종류가 늘어날 경우, Maker를 따로 만들 예정


public class BlockMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject GlassPrefab; // GlassPrefab
    [SerializeField]
    private GameObject StonePrefab; // StonePrefab
    [SerializeField]
    private int Test_GlassSpawnCount = 5; // 테스트용 GlassPrefab의 초기 생성 개수, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    [SerializeField]
    private int Test_StoneSpawnCount = 5; // 테스트용 StonePrefab의 초기 생성 개수, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    [SerializeField]
    private float Test_respawnTime = 5f; // 테스트용 재생성 시간, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    [SerializeField]
    private Vector2 map_left_down= new Vector2(0,0);
    [SerializeField]
    private float map_width = 0.0f;
    [SerializeField]
    private float map_hight = 0.0f;

    Vector2 spawnPosition;

    void Start()
    {
        LoadPrefabs();
        SpawnResources();
    }

    void LoadPrefabs(){
        GlassPrefab = Resources.Load<GameObject>("Prefabs/Glass");
        StonePrefab = Resources.Load<GameObject>("Prefabs/Stone");
    }

    void SpawnResources()
    {
        for (int i = 0; i < Test_GlassSpawnCount; i++)
        {
            spawnPosition = GetRandomSpawnPosition();
            GameObject glass = Instantiate(GlassPrefab, spawnPosition, Quaternion.identity); // 회전이 없는 상태를 나타내는 Quaternion.identity
            glass.tag = "Glass"; // GlassPrefab의 태그 설정;
        }
        // GlassPrefab 초기 생성
        
        for (int i = 0; i < Test_StoneSpawnCount; i++)
        {
            spawnPosition = GetRandomSpawnPosition();
             GameObject stone = Instantiate(StonePrefab, spawnPosition, Quaternion.identity);
            stone.tag = "Stone"; // StonePrefab의 태그 설정
        }
        // StonePrefab 초기 생성
    }

    Vector3 GetRandomSpawnPosition()
    {
        // 랜덤한 위치 계산
        float x = Random.Range(map_left_down.x, map_left_down.x+ map_width); // 위치는 Map_size에 따라 달라질 예정
        float y = Random.Range(map_left_down.y, map_left_down.y+map_hight); // // 위치는 Map_size에 따라 달라질 예정
        return new Vector2(x, y);
    }

    

    public IEnumerator RespawnResources(string destroy_ob)
    {
        //yield return new WaitForSecondsRealtime(Test_respawnTime); // respawnTime만큼 시간 대기
        
        if(destroy_ob=="Glass")
        {
            SpawnGlassResources(); 
        } 
        else if(destroy_ob == "Stone")
        {
            SpawnStoneResources();
        }
        yield return null;

    }// 부서진 object가 뭐냐에 따라 새로 spawn


    void SpawnGlassResources()
    {
        // GlassPrefab 생성
            spawnPosition = GetRandomSpawnPosition();
           GameObject glass = Instantiate(GlassPrefab, spawnPosition, Quaternion.identity);
            glass.tag = "Glass"; // 없어진 후 새로 생성되는 GlassPrefab의 태그 설정
    }

    void SpawnStoneResources()
    {
        // StonePrefab 생성
            spawnPosition = GetRandomSpawnPosition();
            GameObject stone = Instantiate(StonePrefab, spawnPosition, Quaternion.identity);
            stone.tag = "Stone"; // 없어진 후 새로 생성되는 StonePrefab의 태그 설정
    }
    
}

