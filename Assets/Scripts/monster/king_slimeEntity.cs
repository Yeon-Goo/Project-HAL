using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king_slimeEntity : MonoBehaviour
{
    public GameObject rupinPrefab;
    public GameObject BananaPrefab;
    public Transform player;
    public float speed = 2f;
    public float range = 10f;
    public float barrage_time = 3f;
    public float rush_time = 5f;
    public float summon_time = 3f;
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;
    int barrage_count = 4;

    public static Vector3 AngleToVector(float angleDegrees)
    {
        // 각도를 라디안으로 변환
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // 각도를 사용하여 벡터 계산
        float x = Mathf.Cos(angleRadians);
        float y = Mathf.Sin(angleRadians);

        // 2D 벡터 생성 (Z는 0으로 설정)
        return new Vector3(x, y, 0);
    }
    private Animator animator;
    private static string animationState = "AnimationState";

    enum StateEnum
    {
        idle = 0,
        move = 1,
        barrage = 2,
        rush = 3,
        summon = 4,
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        monster_state = (int)StateEnum.idle;
        transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
        animator.SetInteger(animationState, monster_state);
    }
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(monster_state == (int)StateEnum.idle) // 평상시
        {
            if(distance < range) // 거리가 가까워지면 추격상태로 전환
            {
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.move) // 추격 중
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if(Time.time - tempTime > 3.0f)
            {
                int rand = Random.Range(1, 11);
                if(rand < 0)
                {
                    monster_state = (int)StateEnum.barrage;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
                else if(rand < 0)
                {
                    monster_state = (int)StateEnum.rush;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
                else
                {
                    monster_state = (int)StateEnum.summon;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
            }
        }

        if (monster_state == (int)StateEnum.barrage) // 탄막 피하기
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
            // TODO : 탄막 나오는 알고리즘
            if(Time.time - tempTime >= 0.3f*barrage_count)
            {
                float rand = Random.Range(12, 24);
                for(int i = 0; i < 10; i++)
                {
                    GameObject Banana = Instantiate(BananaPrefab);
                    Banana.transform.position = transform.position;
                    Vector3 NewDirection = AngleToVector(36*i + 18*barrage_count + rand);
                    Banana.GetComponent<Rigidbody2D>().AddForce((NewDirection).normalized * 800);
                }
                barrage_count++;
            }
            if(Time.time - tempTime >= barrage_time) // 탄막 지속시간이 지났으면 추격 상태로 전환
            {
                barrage_count = 4;
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
        }
        
        if (monster_state == (int)StateEnum.rush) // 돌진
        {
            // TODO : 돌진하는 알고리즘
            if(Time.time - tempTime > rush_time) // 공격이 끝나고 딜레이로 전환
            {
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, (int)StateEnum.idle);
            }
        }

        if(monster_state == (int)StateEnum.summon) // 소환
        {
            // TODO : 소환하는 알고리즘
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(Time.time - tempTime > summon_time) // 딜레이 시간이 끝나고
            {
                GameObject rupin = Instantiate(rupinPrefab);
                rupin.transform.position = transform.position;
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, (int)StateEnum.idle);
            }
        }
    }
}