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
    private Vector2 player_pos;


    private Animator animator;
    private static string animationState = "AnimationState";

    enum StateEnum
    {
        idle = 0,
        move = 1,
        charge = 2,
        attack = 3,
        delay = 4
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(monster_state == (int)StateEnum.idle) // 평상시
        {
            if(distance < range) // 거리가 가까워지면 추격상태로 전환
            {
                monster_state = (int)StateEnum.move;
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
                player_pos = new Vector2(player.position.x, player.position.y);
                monster_state = (int)StateEnum.charge;
                tempTime = Time.time;
            }
            if(distance > tracking_range) // 시야 사거리가 멀어졌을 경우 평상시로 전환
            {
                monster_state = (int)StateEnum.idle;
            }
        }

        if (monster_state == (int)StateEnum.charge) // 차지 중
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 대기
            if(Time.time - tempTime >= attack_charge) // 차지시간이 지났으면 공격상태로 전환
            {
                tempTime = Time.time;
                monster_state = (int)StateEnum.attack;
            }
        }
        
        if (monster_state == (int)StateEnum.attack) // 공격
        {
            transform.position = Vector2.MoveTowards(transform.position, player_pos, attack_speed * Time.deltaTime);
            if(Time.time - tempTime > attack_time) // 공격이 끝나고 딜레이로 전환
            {
                monster_state = (int)StateEnum.delay;
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
                }
                else  // 돌진 후 멀면 평상시로 전환
                {
                    monster_state = (int)StateEnum.idle;
                }
            }
        }
    }
}