using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

// Player의 움직임에 대한 Controller

public class PlayerEntityMovementController : MonoBehaviour
{
    // Player의 속력
    private float velocity = 3.0f;
    // Player의 방향
    private Vector2 vector = new Vector2();
    // Player가 움직일 수 있는지의 유무 (stop 기능에 쓰임)
    private bool is_moveable = false;
    // Player의 목표 좌표
    private Vector2 mousePos = new Vector2();

    // Component Variables
    private UnityEngine.Transform transform;
    private Animator animator;
    private Rigidbody2D rigidbody;

    // String Variables
    private static string animationState = "AnimationState";

    // Player의 애니메이션 재생에 쓰이는 변수
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
    public bool debug_is_moveable;
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
        if (!vector.Equals(Vector2.zero))
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
        // S키를 눌렀을 때 캐릭터가 멈춤
        if (Input.GetKey(KeyCode.S))
        {
            is_moveable = false;
            mousePos = transform.position;
        }

        //MoveCharacter_KeyBoard();
        MoveCharacter_Mouse();
        //MoveCharacter_Mouse_STOP_WHEN_RELEASED();
    }

    //UNUSED
    /* 
    private void MoveCharacter_KeyBoard()
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
    */

    private void MoveCharacter_Mouse()
    {
        // 마우스 오른쪽 버튼을 누르고 있을 때 && 마우스 좌표에 변화가 생길 때
        if (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)
        {
            is_moveable = true;
        }

        // S키를 누르지 않았을 때
        if(Input.GetMouseButton(1) && is_moveable)
        //if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        vector.x = mousePos.x - transform.position.x;
        vector.y = mousePos.y - transform.position.y;

        if (vector.magnitude < 0.1f)
        {
            vector = Vector2.zero;
        }

        vector.Normalize();

        rigidbody.velocity = vector * velocity;

#if DEBUG
        debug_vector = vector;
        debug_vector_x = debug_vector.x;
        debug_is_moveable = is_moveable;
#endif
    }

//UNUSED
/* 
private void MoveCharacter_Mouse_STOP_WHEN_RELEASED()
{
    vector = Vector2.zero;

    if (Input.GetMouseButton(1))
    {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    vector.x = mousePos.x - transform.position.x;
    vector.y = mousePos.y - transform.position.y;

    //Debug.Log("Vector Magnitude : " + vector.magnitude + "  (" + vector.x + ", " + vector.y + ")");

    if (vector.magnitude < 0.05f)
    {
        vector = Vector2.zero;
    }

    vector.Normalize();
    }

    rigidbody.velocity = vector * velocity;

#if DEBUG
    debug_vector = vector;
    debug_vector_x = debug_vector.x;
#endif
}
*/
}
