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
    public float attack_time = 3f;
    public float attack_delay = 5f;
    private bool isCharging = false;
    private bool isDelay = false;
    private bool isTracking = false;
    private float tempTime = 0f;


    private Animator animator;
    private static string animationState = "AnimationState";

    enum AnimationStateEnum
    {
        idle = 0,
        move = 1,
        charge = 2,
        attack = 3,
        delay = 4
    }

#if DEBUG
    public float debug_velocity;
    public Vector2 debug_vector;
    public float debug_vector_x;
    public bool debug_is_moveable;
    public int debug_animationstate;
#endif


    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if(!isTracking && distance < range)
        {
            isTracking = true;
        }
        if (distance <= tracking_range && distance > attack_range && !isCharging && !isDelay && isTracking) // 추격 중
        {
            //transform.LookAt(player);
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (distance <= attack_range && !isCharging && !isDelay) // 공격 대기
        {
            //transform.LookAt(player);
            isCharging = true;
            tempTime = Time.time;
        }
        if (isCharging && Time.time - tempTime > attack_charge && !isDelay) // 공격
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attack_speed * Time.deltaTime);
            if(Time.time - tempTime > attack_time)
            {
                isDelay = true;
            }
        }
        if (isCharging && Time.time - tempTime <= attack_charge && !isDelay) // 공격 선딜
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if(isDelay) // 공격 후딜
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if(Time.time - tempTime > attack_delay)
            {
                isDelay = false;
                isCharging = false;
            }
        }
        if(isTracking && !isDelay && !isCharging && distance > tracking_range)
        {
            isTracking = false;
        }
    }
}