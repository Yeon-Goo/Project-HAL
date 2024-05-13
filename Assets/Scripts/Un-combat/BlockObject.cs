using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BlockObject : MonoBehaviour
{

    private string destroy_tag;
    private float lastContactTime;
    [SerializeField]
    protected float Test_contactDuration = 2f; // 테스트용 자원 채집하는데 걸리는 시간, 이는 맵의 크기 및 난이도 조정에 따라 차후 결정 예정
    
    private BlockMaker blockMaker;


    void Start()
    {
        blockMaker = FindObjectOfType<BlockMaker>(); // Scene에서 BlockMaker 클래스의 인스턴스를 찾음
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastContactTime = Time.time; // 접촉한 시간 기록
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastContactTime = 0f; // 접촉이 끝났으므로 시간 초기화
        }
    }

    void Update()
    {
        if ((Time.time - lastContactTime) >= Test_contactDuration && lastContactTime != 0f)
        {
            Debug.Log("here1");
            destroy_tag = gameObject.tag; // 태그 수정해서, Un-combat으로 통일하고 그 안에서 프리팹 이름으로 구분해야 할듯
            StartCoroutine(blockMaker.RespawnResources(destroy_tag));
            Destroy(gameObject);
            
        }
    }

}
