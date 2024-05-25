using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king_slimeEntity : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject rupinPrefab;
    public GameObject BananaPrefab;
    public Transform player;
    public float speed = 2f;
    public float range = 10f;
    public float barrage_time = 4f;
    public float rush_time = 5f;
    public float summon_time = 3f;
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;
    int barrage_count = 4;
    float r;
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        monster_state = (int)StateEnum.idle;
        transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
        animator.SetInteger(animationState, monster_state);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("UpWall"))
        {
            // 랜덤하게 슬라임이 발사됨
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(210, 330);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("DownWall"))
        {
            // 랜덤하게 슬라임이 발사됨
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(30, 150);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("LeftWall"))
        {
            // 랜덤하게 슬라임이 발사됨
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(-30, 30);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("RightWall"))
        {
            // 랜덤하게 슬라임이 발사됨
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(150, 210);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Blocking") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemies")) // 'Blocking' 레이어의 오브젝트와 충돌했을 때
        {
            Vector2 incomingVector = rb.velocity; // 입사 벡터는 현재 속도 벡터
            Vector2 normalVector = collision.contacts[0].normal; // 충돌 지점의 첫 번째 접촉 정보로부터 법선 벡터를 얻음
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normalVector).normalized; // 반사 벡터 계산

            float speedReduction = 0.3f; // 감소할 속도의 크기
            float newSpeed = Mathf.Max(0, incomingVector.magnitude - speedReduction); // 새로운 속도 크기 계산 (0보다 작아지지 않도록)

            rb.velocity = reflectVector * newSpeed; // 반사 방향으로 새로운 속도를 업데이트
        }
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
                int rand = Random.Range(1, 15);
                if(rand < 8)
                {
                    monster_state = (int)StateEnum.barrage;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
                else if(rand < 14)
                {
                    monster_state = (int)StateEnum.rush;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                    r = Random.Range(0, 360);
                    r = 270;
                    Vector3 NewDirection = AngleToVector(r);
                    GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
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
                    Banana.GetComponent<Rigidbody2D>().AddForce((NewDirection).normalized * 900);
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
            if(Time.time - tempTime > rush_time) // 돌진이 끝나고 추격상태로 전환
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
            if(Time.time - tempTime > summon_time) // 소환 후 추격상태로 전환
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