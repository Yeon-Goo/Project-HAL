using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerEntity : Entity
{
    // Entity의 Stat을 관리하는 변수
    //public StatManager stat_manager;

    // String Variables
    private static string animationState = "AnimationState";
    private static string pickable_objects = "Pickable_Objects";

    // Player's UIs
    private InventoryUI inventory_prefab;
    private InventoryUI inventory_ui;
    private PlayerDeck card_prefab;
    private PlayerDeck card_ui;


    /***
     * Player의 좌표 관련 변수들
     ***/
    [SerializeField]
    // Player의 속력
    private float velocity = 3.0f;
    [SerializeField]
    // Player가 움직일 목표 좌표
    private Vector2 target_pos = new Vector2();
    [SerializeField]
    // Player가 움직일 방향
    private Vector2 vector = new Vector2();


    /***
    * Player의 Animation 관련 변수들
    ***/
    Coroutine animation_coroutine = null;
    // Player가 현재 오른쪽을 바라보는지
    public bool is_looking_right = true;
    [SerializeField]
    // Player의 애니메이션이 재생 중인지
    public bool is_animation_playing = false;
    [SerializeField]
    public bool is_animation_ended = true;
    [SerializeField]
    // Player가 현재 움직일 수 있는 상황인지
    private bool is_moveable = false;
    // Player가 살아 있는지
    private bool is_alive = true;
    [SerializeField]
    // Player가 무적인지
    private bool is_invincible = false;
    [SerializeField]
    // Player의 무적 시간
    private float interval = 0.0f;


    /***
     * Player의 Components
     ***/
    private Animator animator;
    private new Rigidbody2D rigidbody;

    // Player가 현재 어떤 애니메이션을 재생해야 하는지 저장하는 변수
    enum AnimationStateEnum
    {
        idle = 0,
        walk = 1,
        roll = 2,
        attack = 3,
        damaged = 4,
        dead = 5

    }
    void Start()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/PlayerHPManager");
        if (hp_manager == null) return;

        // Load StatManager
        //stat_manager = Resources.Load<StatManager>("ScriptableObjects/StatManager");
        //if (stat_manager == null) return;

        // Load HPBarUI Prefab
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/PlayerHPBarUI");
        if (hpbar_prefab == null) return;
        // Instantiate HPBarUI
        hpbar_ui = Instantiate(hpbar_prefab);
        // HPBarUI를 this의 자식으로 생성
        hpbar_ui.transform.SetParent(this.transform, false);
        if (hpbar_ui == null) return;
        hpbar_ui.Init(this);

        // Load InventoryUI Prefab
        inventory_prefab = Resources.Load<InventoryUI>("Prefabs/UI/Inventory/InventoryUI");
        if (inventory_prefab == null) return;
        // Instantiate InventoryUI
        inventory_ui = Instantiate(inventory_prefab);
        // InventoryUI를 this의 자식으로 생성
        inventory_ui.transform.SetParent(this.transform, false);
        if (inventory_ui == null) return;

        /*
        // Load CardUI Prefab
        card_prefab = Resources.Load<PlayerDeck>("Prefabs/UI/Card/CardUI");
        if (card_prefab == null) return;
        // Instantiate CardUI
        card_ui = Instantiate(card_prefab);
        // CardUI를 this의 자식으로 생성
        card_ui.transform.SetParent(this.transform, false);
        if (card_ui == null) return;
        */

        // Get Components
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
        if (Input.GetKey(KeyCode.H))
        {
            ResetEntity();
        }
    }

    void FixedUpdate()
    {
        // 무적 시간 계산
        if (is_invincible)
        {
            interval -= Time.deltaTime;
            if (interval < float.Epsilon)
            {
                is_invincible = false;
                this.interval = 0.0f;
            }
        }

        if (is_alive && !is_animation_playing)
        {
            // Stop Player
            if (Input.GetKey(KeyCode.S))
            {
                CharacterStop();
            }
            // Roll
            else if (Input.GetKey(KeyCode.Space))
            {
                if (animation_coroutine == null)
                {
                    animation_coroutine = StartCoroutine(Roll(animator));
                }
                animation_coroutine = null;
            }
            // Roll
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                if (animation_coroutine == null)
                {
                    animation_coroutine = StartCoroutine(Attack(animator));
                }
                animation_coroutine = null;
            }
            // Walk
            else
            {
                MoveCharacter_Mouse();
                vector = target_pos - new Vector2(transform.position.x, transform.position.y);
            }
        }

        // Player의 벡터 정규화
        if (vector.magnitude < 0.1f)
        {
            vector = Vector2.zero;
        }
        else
        {
            vector.Normalize();
        }

        // 캐릭터의 벡터가 0이 아니면 움직이고 있는 상태
        if (!vector.Equals(Vector2.zero))
        {
            // x 방향으로 움직였을 때, x 성분의 값으로 오른쪽 왼쪽을 판단
            if (vector.x != 0) is_looking_right = vector.x > 0 ? true : false;
            // x 방향으로 움직이지 않았을 때, transform.localScale.x의 값으로 오른쪽 왼쪽을 판단
            else is_looking_right = transform.localScale.x.Equals(1.0f) ? true : false;
            // 캐릭터가 바라보는 방향을 움직이는 방향에 맞게 바꿈
            transform.localScale = is_looking_right ? new Vector3(1.0f, 1.0f, 1.0f) : new Vector3(-1.0f, 1.0f, 1.0f);
        }

        // 캐릭터를 vector와 velocity에 맞게 움직임
        rigidbody.velocity = vector * velocity;

        UpdateAnimationState();
    }

    public void CharacterStop()
    {
        is_moveable = false;
        target_pos = transform.position;
        vector = Vector2.zero;
    }

    IEnumerator Roll(Animator animator)
    {
        is_animation_playing = true;
        velocity = 4.5f;
        target_pos = GetMousePos();
        vector = target_pos - GetPos();
        vector.Normalize();

        animator.SetInteger(animationState, (int)AnimationStateEnum.roll);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        velocity = 3.0f;
        CharacterStop();
        animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
        is_animation_playing = false;
    }

    IEnumerator Attack(Animator animator)
    {
        is_animation_playing = true;
        CharacterStop();

        animator.SetInteger(animationState, (int)AnimationStateEnum.attack);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
        is_animation_playing = false;
    }

    IEnumerator Damaged(Animator animator)
    {
        is_animation_playing = true;
        
        animator.SetInteger(animationState, (int)AnimationStateEnum.damaged);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        CharacterStop();
        is_animation_playing = false;
    }

    private void UpdateAnimationState()
    {
        if (!is_animation_playing)
        {
            // WALK
            if (!vector.Equals(Vector2.zero))
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.roll);
                is_animation_ended = false;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.attack);
                is_animation_ended = false;
            }
            // IDLE
            else
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
            }
        }
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
            // Player의 벡터 계산
            target_pos = FindPath();
        }
    }

    // 마우스 오른쪽 버튼을 눌렀을 때의 좌표까지 도달할 수 있는 최단 거리 탐색
    private Vector2 FindPath()
    {
        // 지금은 단순히 마우스 오른쪽 버튼을 눌렀을 때의 좌표를 return
        return GetMousePos();

        // 나중에 길찾기 알고리즘을 구현해야 함
    }

    // UN-COMBAT 물체와 충돌했을 경우 일단은 정지하도록 설정. 애니메이션 설정 나중에 하면 될듯?
    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.CompareTag("Glass") || col.gameObject.CompareTag("Stone"))
        {
            Debug.Log("here");
            CharacterStop();
        }
    } 



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if (hitObject != null)
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
    public override IEnumerator DamageEntity(int damage, float interval, GameObject entity)
    {
        //float cur_hp = hp_manager.GetCurrentHP();

        while (true)
        {
            if (!is_invincible)
            {
                is_invincible = true;
                this.interval = interval;

                // this는 entity로부터 damage만큼의 피해를 interval초마다 받는다
                CharacterStop();
                StartCoroutine(FlickEntity());

                //GetComponent<CinemachineVirtualCamera>().VibrateForTimeAndAmount();

                hp_manager.Cur_hp -= damage;
            }

            // this의 체력이 0일 때
            //if (cur_hp <= float.Epsilon)
            if (hp_manager.Cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

            // this의 체력이 0보다 크면 interval만큼 실행을 양보(멈춤)
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
        CharacterStop();
        is_alive = false;
        hp_manager.Cur_hp = 0;
        //GetComponent<SpriteRenderer>().enabled = false;
        animator.SetInteger(animationState, (int)AnimationStateEnum.dead);
        //GameManager.sharedInstance.SpawnPlayer();
        // 미완성
    }

    // 플레이어가 부활하는 코드
    public override void ResetEntity()
    {
        is_alive = true;
        hp_manager.Cur_hp = hp_manager.Max_hp;
        hp_manager.Cur_mp = hp_manager.Max_mp;
        GetComponent<SpriteRenderer>().enabled = true;
        // 미완성
    }
}
