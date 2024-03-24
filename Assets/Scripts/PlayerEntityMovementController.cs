using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

// Player�� �����ӿ� ���� Controller

public class PlayerEntityMovementController : MonoBehaviour
{
    [SerializeField]
    // Player�� �ӷ�
    private float velocity = 3.0f;
    // Player�� ����
    private Vector2 vector = new Vector2();
    // Player�� ������ �� �ִ����� ���� (stop ��ɿ� ����)
    private bool is_moveable = false;
    // Player�� ��ǥ ��ǥ
    private Vector2 target_pos = new Vector2();

    // Component Variables
    private UnityEngine.Transform transform;
    private Animator animator;
    private Rigidbody2D rigidbody;

    // String Variables
    private static string animationState = "AnimationState";

    // Player�� �ִϸ��̼� ����� ���̴� ����
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
            // x �� �Է��� �����ϴ� ���
            // x �� �Է¿� ���� �������� �ٶ���, ������ �ٶ��� ����
            if (vector.x > 0)
            {
                // �������� �ٶ�
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
            }
            else if (vector.x < 0)
            {
                // ������ �ٶ�
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_left);
            }
            // x �� �Է��� �������� �ʴ� ���
            // transform�� localScale�� x ���� ���� �������� �ٶ���, ������ �ٶ��� ����
            else
            {
                if (transform.localScale.x.Equals(1.0f))
                {
                    // �������� �ٶ�
                    animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
                }
                else
                {
                    // ������ �ٶ�
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
        // SŰ�� ������ �� ĳ���Ͱ� ����
        if (Input.GetKey(KeyCode.S))
        {
            is_moveable = false;
            target_pos = transform.position;
        }

        //MoveCharacter_KeyBoard();
        MoveCharacter_Mouse();
        //MoveCharacter_Mouse_STOP_WHEN_RELEASED();
    }

    private void MoveCharacter_Mouse()
    {
        // ���콺 ������ ��ư�� ������ ���� �� || (���콺 ������ ��ư�� ������ ���� �� && ���콺 ��ǥ�� ��ȭ�� ���� ��)
        if (!Input.GetMouseButton(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        //if (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)
        {
            is_moveable = true;
        }

        // SŰ�� ������ �ʾ��� ��
        if (is_moveable && Input.GetMouseButton(1))
        //if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        {
            target_pos = FindPath();
        }

        vector.x = target_pos.x - transform.position.x;
        vector.y = target_pos.y - transform.position.y;

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

    private Vector2 FindPath()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
}
