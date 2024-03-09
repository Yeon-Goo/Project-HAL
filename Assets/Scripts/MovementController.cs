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
