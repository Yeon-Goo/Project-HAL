using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerEntity : Entity
{
    // String Variables
    private static string animationState = "AnimationState";
    private static string pickable_objects = "Pickable_Objects";

    private HPBarUI hpbar_prefab;
    private HPBarUI hpbar_ui;
    private InventoryUI inventory_prefab;
    private InventoryUI inventory_ui;
    //private CardUI card_prefab;
    //private CardUI card_ui;

    // Player의 속력
    private float velocity = 3.0f;
    // Player가 움직일 방향
    private Vector2 vector = new Vector2();
    // Player가 현재 움직일 수 있는 상황인지
    private bool is_moveable = false;
    // Player가 움직일 목표 좌표
    private Vector2 target_pos = new Vector2();
    // Player가 살아 있는지
    private bool is_alive = true;

    // Component Variables
    private new UnityEngine.Transform transform;
    private Animator animator;
    private new Rigidbody2D rigidbody;

    // Player가 현재 어떤 애니메이션을 재생해야 하는지 저장하는 변수
    enum AnimationStateEnum
    {
        idle = 0,
        walk_right = 1,
        walk_left = 2,
    }
    void Start()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/PlayerHPManager");
        if (hp_manager == null) return;

        // Load HPBarUI Prefab
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/HPBarUI");
        if (hpbar_prefab == null) return;
        // Instantiate HPBarUI
        hpbar_ui = Instantiate(hpbar_prefab);
        if (hpbar_ui == null) return;
        hpbar_ui.Init(this);

        // Load InventoryUI Prefab
        inventory_prefab = Resources.Load<InventoryUI>("Prefabs/UI/Inventory/InventoryUI");
        if (inventory_prefab == null) return;
        // Instantiate InventoryUI
        inventory_ui = Instantiate(inventory_prefab);
        if (inventory_ui == null) return;

        /*
        // Load CardUI Prefab
        card_prefab = Resources.Load<CardUI>("Prefabs/UI/Card/CardUI");
        if (card_prefab == null) return;
        // Instantiate CardUI
        card_ui = Instantiate(card_prefab);
        if (card_ui == null) return;
        */

        // Get Components
        transform = GetComponent<UnityEngine.Transform>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        //ResetEntity();
    }

    /*
    private void OnEnable()
    {
        ResetEntity();
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (is_alive) UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (!vector.Equals(Vector2.zero))
        {
            // x 방향으로 움직였을 때
            // x 성분의 값으로 오른쪽 왼쪽을 판단
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
            // x 방향으로 움직이지 않았을 때
            // transform.localScale.x의 값으로 오른쪽 왼쪽을 판단
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
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.H))
        {
            ResetEntity();
        }

        if (is_alive)
        {
            // S키를 입력하면 캐릭터가 그 자리에서 멈춤
            if (Input.GetKey(KeyCode.S))
            {
                CharacterStop();
            }
            //MoveCharacter_KeyBoard();
            MoveCharacter_Mouse();
            //MoveCharacter_Mouse_STOP_WHEN_RELEASED();
        }
    }

    public void CharacterStop()
    {
        is_moveable = false;
        target_pos = transform.position;
        //Debug.Log("CALLED");
    }

    private void MoveCharacter_Mouse()
    {
        // 마우스 오른쪽 버튼을 뗐을 때 || 마우스 오른쪽 버튼을 누르고 있는 상태에서 마우스를 움직였을 때
        if (!Input.GetMouseButton(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        //if (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)
        {
            is_moveable = true;
        }

        // S키를 누르지 않았고 마우스 오른쪽 버튼을 눌렀을 때
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
    }

    // 마우스 오른쪽 버튼을 눌렀을 때의 좌표까지 도달할 수 있는 최단 거리 탐색
    private Vector2 FindPath()
    {
        // 지금은 단순히 마우스 오른쪽 버튼을 눌렀을 때의 좌표를 return
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 나중에 길찾기 알고리즘을 구현해야 함
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if(hitObject != null)
            {
                bool should_disappear = false;

                //print("Hit: " + hitObject.GetName());
                switch (hitObject.GetItemType())
                {
                    case Item.ItemTypeEnum.COIN:
                        should_disappear = inventory_ui.AddItem(hitObject);
                        break;
                    case Item.ItemTypeEnum.HEALTH:
                        should_disappear = hp_manager.AdjustHP(hitObject.GetQuantity());
                        break;
                }

                if (should_disappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    // 플레이어가 대미지를 받는 함수
    public override IEnumerator DamageEntity(int damage, float interval, Entity entity)
    {
        float cur_hp = hp_manager.GetCurrentHP();

        while (true)
        {
            // 플레이어는 entity로부터 damage만큼의 피해를 interval초마다 받는다
            Debug.Log("Player Get " + damage + " Damage From " + entity.name + "(interval : " + interval + ")\n");
            cur_hp -= damage;
            hp_manager.SetCurrentHP(cur_hp);

            // 플레이어의 체력이 0일 때
            if (cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

            // 플레이어의 체력이 0보다 크면 interval만큼 실행을 양보
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    // 플레이어가 사망하는 코드
    override public void KillEntity()
    {
        is_alive = false;
        hp_manager.SetCurrentHP(0);
        GetComponent<SpriteRenderer>().enabled = false;
        // 미완성
    }

    // 플레이어가 부활하는 코드
    public override void ResetEntity()
    {
        is_alive = true;
        hp_manager.SetCurrentHP(hp_manager.GetMaxHP());
        GetComponent<SpriteRenderer>().enabled = true;
        // 미완성
    }
}