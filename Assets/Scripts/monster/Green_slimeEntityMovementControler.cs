using UnityEngine;
using Spine.Unity;

public class MonsterTracking : MonoBehaviour
{
    private Transform player;
    public float speed = 2f;
    public float range = 4f;
    public float tracking_range = 7f;
    public float attack_range = 2f;
    public float attack_speed = 5f;
    public float attack_time = 0.7f;
    public float idle_delay = 1f; // Idle 상태 유지 시간
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;
    private bool after_Attack = false;

    private Rigidbody2D rb;
    private SkeletonAnimation skeletonAnimation;
    private static string animationState = "AnimationState";

    enum StateEnum
    {
        Idle = 0,
        Walk = 1,
        PrepareAttack = 2,
        Attack = 3
    }

    void Start()
    {
        GameObject playerObject = GameObject.Find("PlayerObject");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        initial_pos = transform.position;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D를 가져옴
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (monster_state == (int)StateEnum.Idle) // 평상시
        {
            if(after_Attack == false)
            {
                if (distance < range) // 거리가 가까워지면 추격상태로 전환
                {
                    monster_state = (int)StateEnum.Walk;
                    SetAnimation("Walk", true);
                }
                else if (Time.time - tempTime > 1f) // 평상시 상태가 1초 이상 지속시 탐색상태로 전환
                {
                    wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    Vector2 From_Current_To_Initial = (initial_pos - (Vector2)transform.position).normalized;
                    move_direction = (From_Current_To_Initial + wanderDirection).normalized;
                    target_pos = transform.position + (Vector3)move_direction * 10;
                    tempTime = Time.time;
                    monster_state = (int)StateEnum.Walk;
                    SetAnimation("Walk", true);
                }
            }
            else if (Time.time - tempTime > 0.5f)
            {
                after_Attack = false;
            }
        }

        if (monster_state == (int)StateEnum.Walk) // 추격 중
        {
            if (distance <= tracking_range && distance > attack_range) // 공격 사거리가 안 될 시
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                FlipSprite(); // 스프라이트의 방향을 지속적으로 업데이트
            }
            if (distance <= attack_range) // 공격 사거리가 됐을 경우 공격 준비 상태로 전환
            {
                move_direction = (player.position - transform.position).normalized;
                monster_state = (int)StateEnum.PrepareAttack;
                SetAnimation("Idle", true);
                tempTime = Time.time;
            }
            if (distance > tracking_range) // 시야 사거리가 멀어졌을 경우 평상시로 전환
            {
                monster_state = (int)StateEnum.Idle;
                SetAnimation("Idle", true);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.PrepareAttack) // 공격 준비
        {
            rb.velocity = Vector2.zero; // 움직임을 멈춤
            if (Time.time - tempTime > 0.5f) // 0.5초 동안 대기
            {
                rb.AddForce(move_direction * attack_speed, ForceMode2D.Impulse); // 플레이어 방향으로 힘을 가함
                monster_state = (int)StateEnum.Attack;
                SetAnimation("Attack", false);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.Attack) // 공격
        {
            if (Time.time - tempTime > attack_time) // 공격 애니메이션이 끝나고
            {
                rb.velocity = Vector2.zero;
                monster_state = (int)StateEnum.Idle; // Idle 상태로 전환하여 1초 유지
                SetAnimation("Idle", true);
                after_Attack = true;
                tempTime = Time.time;
            }
        }
    }

    private void FlipSprite()
    {
        // 플레이어의 위치와 몬스터의 위치를 비교하여 스케일을 조정합니다.
        if (player != null)
        {
            if (transform.position.x > player.position.x)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // 오른쪽에 있을 경우 y축 대칭
            }
            else
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 왼쪽에 있을 경우 정상
            }
        }
    }

    private void SetAnimation(string animationName, bool loop)
    {
        skeletonAnimation.state.SetAnimation(0, animationName, loop);
    }
}