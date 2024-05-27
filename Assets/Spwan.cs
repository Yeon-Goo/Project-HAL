using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monsterPrefab;  // 생성할 몬스터의 프리팹
    public Transform player;  // 플레이어의 트랜스폼
    public float spawnInterval = 7f;  // 스폰 간격 (초)
    public int maxMonsterCount = 3;  // 최대 몬스터 수
    public float spawnRange = 2f;  // 스폰 범위 (x, y ± 값)
    public float activationDistance = 20f;  // 스폰 활성화 거리
    public float deactivationDistance = 30f;  // 스폰 비활성화 거리

    private int currentMonsterCount = 0;  // 현재 생성된 몬스터 수
    private float nextSpawnTime = 0f;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 활성화 거리 내에 있을 때 스폰을 시도합니다.
        if (distanceToPlayer <= activationDistance)
        {
            if (Time.time >= nextSpawnTime && currentMonsterCount < maxMonsterCount)
            {
                SpawnMonster();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        // 플레이어가 비활성화 거리 밖으로 나갔을 때 스폰을 중지합니다.
        if (distanceToPlayer > deactivationDistance)
        {
            // 필요 시 추가적인 비활성화 로직을 여기에 추가 가능
            // 예를 들어, 현재 스폰된 몬스터들을 제거하는 로직
        }
    }

    void SpawnMonster()
    {
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnRange, spawnRange),
            transform.position.y + Random.Range(-spawnRange, spawnRange),
            transform.position.z
        );

        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        currentMonsterCount++;

        // 몬스터가 죽을 때 몬스터 수를 감소시키는 이벤트를 등록합니다.
        monster.GetComponent<Entity>().OnDeath += HandleMonsterDeath;
    }

    void HandleMonsterDeath()
    {
        currentMonsterCount--;
    }
}