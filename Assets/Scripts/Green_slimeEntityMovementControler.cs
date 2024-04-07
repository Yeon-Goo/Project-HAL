using UnityEngine;

public class MonsterTracking : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float range = 4f;
    public float tracking_range = 7f;
    public float attack_range = 2f;
    public float attack_speed = 5f;
    public float attack_charge = 2f;
    public float attack_time = 1f;
    public float attack_delay = 2f;
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;


    private Animator animator;
    private static string animationState = "AnimationState";

    enum StateEnum
    {
        idle = 0,
        move = 1,
        charge = 2,
        attack = 3,
        delay = 4,
        idle_move = 5
    }
    void Start()
    {
        initial_pos = transform.position;
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(monster_state == (int)StateEnum.idle) // 평상시
        {
            if(distance < range) // 거리가 가까워지면 추격상태로 전환
            {
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
            else if(Time.time - tempTime > 1f) // 평상시 상태가 1초 이상 지속시 탐색상태로 전환
            {
                wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                Vector2 From_Current_To_Initial = (initial_pos - (Vector2)transform.position).normalized;
                move_direction = (From_Current_To_Initial + wanderDirection).normalized;
                target_pos = transform.position + (Vector3)move_direction * 10;
                tempTime = Time.time;
                monster_state = (int)StateEnum.idle_move;
            }
        }

        if (monster_state == (int)StateEnum.move) // 추격 중
        {
            if(distance <= tracking_range && distance > attack_range) // 공격 사거리가 안 될 시
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            if (distance <= attack_range) // 공격 사거리가 됐을 경우 차지상태로 전환
            {
                move_direction = (player.position - transform.position).normalized;
                target_pos = transform.position + (Vector3)move_direction * 3;
                monster_state = (int)StateEnum.charge;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
                tempTime = Time.time;
            }
            if(distance > tracking_range) // 시야 사거리가 멀어졌을 경우 평상시로 전환
            {
                monster_state = (int)StateEnum.idle;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.charge) // 차지 중
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 대기
            if(Time.time - tempTime >= attack_charge) // 차지시간이 지났으면 공격상태로 전환
            {
                tempTime = Time.time;
                monster_state = (int)StateEnum.attack;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
        }
        
        if (monster_state == (int)StateEnum.attack) // 공격
        {
            transform.position = Vector2.MoveTowards(transform.position, target_pos, attack_speed * Time.deltaTime);
            if(Time.time - tempTime > attack_time) // 공격이 끝나고 딜레이로 전환
            {
                monster_state = (int)StateEnum.delay;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
        }

        if(monster_state == (int)StateEnum.delay) // 딜레이 중일 때
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(Time.time - tempTime > attack_delay) // 딜레이 시간이 끝나고
            {
                if(distance < tracking_range)   // 돌진 후 가까우면 추격상태로 전환
                {
                    monster_state = (int)StateEnum.move;
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                }
                else  // 돌진 후 멀면 평상시로 전환
                {
                    monster_state = (int)StateEnum.idle;
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
            }
        }

        if(monster_state == (int)StateEnum.idle_move) // 탐색 상태일 때
        {
            transform.position = Vector2.MoveTowards(transform.position, target_pos, speed * Time.deltaTime);
            if(Time.time - tempTime > 1f)
            {
                tempTime=Time.time;
                monster_state = (int)StateEnum.idle;
            }
        }
    }
}