using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovementController : MonoBehaviour
{
    private float velocity = 3.0f;
    private Vector2 vector = new Vector2();

    private UnityEngine.Transform transform;
    private Animator animator;
    private Rigidbody2D rigidbody;

    string animationState = "AnimationState";
    enum AnimationStateEnum
    {
        idle = 0,
        walk_right = 1,
        walk_left = 2
    }

#if DEBUG
    public float debug_velocity;
    public Vector2 debug_vector;
    public float debug_vector_x;
    public int debug_animationstate;
#endif

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<UnityEngine.Transform>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

#if DEBUG
        debug_velocity = velocity;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if(!vector.Equals(Vector2.zero))
        {
            // x 축 입력이 존재하는 경우
            // x 축 입력에 따라 오른쪽을 바라볼지, 왼쪽을 바라볼지 결정
            if (vector.x > 0)
            {
                // 오른쪽을 바라봄
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
            }
            else if (vector.x < 0)
            {
                // 왼쪽을 바라봄
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_left);
            }
            // x 축 입력이 존재하지 않는 경우
            // transform의 localScale의 x 값에 따라 오른쪽을 바라볼지, 왼쪽을 바라볼지 결정
            else
            {
                if (transform.localScale.x.Equals(1.0f))
                {
                    // 오른쪽을 바라봄
                    animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
                }
                else
                {
                    // 왼쪽을 바라봄
                    animator.SetInteger(animationState, (int)AnimationStateEnum.walk_left);
                }
            }
        }
        else
        {
            animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
        }

#if DEBUG
        debug_animationstate = animator.GetInteger(animationState);
#endif
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        vector.x = Input.GetAxisRaw("Horizontal");
        vector.y = Input.GetAxisRaw("Vertical");

        vector.Normalize();

        rigidbody.velocity = vector * velocity;

#if DEBUG
        debug_vector = vector;
        debug_vector_x = debug_vector.x;
#endif
    }
}
