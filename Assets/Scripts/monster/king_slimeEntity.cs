using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king_slimeEntity : MonoBehaviour
{/*
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
        barrage = 2,
        rush = 3,
        summon = 4,
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
                if(rand < 5)
                {
                    monster_state = (int)StateEnum.barrage;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
                else if(rand < 8)
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
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 대기
            if(Time.time - tempTime >= attack_charge) // 차지시간이 지났으면 공격상태로 전환
            {
                tempTime = Time.time;
                monster_state = (int)StateEnum.attack;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
        }
        
        if (monster_state == (int)StateEnum.rush) // 돌진
        {
            transform.position = Vector2.MoveTowards(transform.position, target_pos, attack_speed * Time.deltaTime);
            if(Time.time - tempTime > attack_time) // 공격이 끝나고 딜레이로 전환
            {
                monster_state = (int)StateEnum.summon;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, (int)StateEnum.idle);
            }
        }

        if(monster_state == (int)StateEnum.summon) // 소환
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(Time.time - tempTime > attack_delay) // 딜레이 시간이 끝나고
            {
                if(distance < tracking_range)   // 돌진 후 가까우면 추격상태로 전환
                {
                    monster_state = (int)StateEnum.move;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                }
            }
        }
    }
*/}